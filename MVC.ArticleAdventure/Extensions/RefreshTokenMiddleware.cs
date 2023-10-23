﻿using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.EntityHelpers.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MVC.ArticleAdventure.Services.Contract;

namespace MVC.ArticleAdventure.Extensions
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthService _authenticationService;

        public RefreshTokenMiddleware(RequestDelegate next,IAuthService authService)
        {
            _next = next;
            _authenticationService=authService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Проверьте срок действия токена здесь
            if (TokenIsExpired(context))
            {
               
                var refreshToken = context.Request.Cookies[CookiesPath.REFRESH_TOKEN];

                if (refreshToken != null)
                {
                    ExecutionResult<CompleteAccessToken> response = await _authenticationService.RefreshToken(refreshToken);
                    if (response.IsSuccess)
                    {
                        var user = await _authenticationService.GetProfile(response.Data.UserNetUid);

                        await SignIn(response.Data,context);
                    }
                }
               
            }

            // Продолжайте обработку запроса в следующем middleware
            await _next(context);
        }
        async Task SignIn(CompleteAccessToken response,HttpContext context)
        {

            var token = new JwtSecurityTokenHandler().ReadToken(response.AccessToken) as JwtSecurityToken;

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", response.AccessToken));
            claims.Add(new Claim("Guid", response.UserNetUid.ToString()));
            claims.Add(new Claim("AccessToken", response.AccessToken.ToString()));
            claims.Add(new Claim("RefreshToken", response.RefreshToken.ToString()));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };


            await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
        }
        private bool TokenIsExpired(HttpContext context)
        {
            var token = context.User.FindFirst("AccessToken");// Получите токен из запроса или контекста
             var handler = new JwtSecurityTokenHandler();

            try
            {
                if (token!= null)
                {

                var jsonToken = handler.ReadToken(token.Value) as JwtSecurityToken;

                if (jsonToken != null)
                {
                    var expirationDate = jsonToken.ValidTo;
                    return expirationDate <= DateTime.UtcNow;
                }
                }
            }
            catch (Exception)
            {
                // Обработка ошибок, связанных с некорректным токеном
                return true; // Считаем токен недействительным в случае ошибки
            }

            return true;
        }
    }
}