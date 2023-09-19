using Azure;
using common.ArticleAdventure.WebApi;
using domain.ArticleAdventure.EntityHelpers.Identity;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace MVC.ArticleAdventure.Controllers
{
    public class IdentityController : MVCControllerBase
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly IAuthService _authenticationService;
        private readonly IUserProfileService _userProfileService;
        private readonly IStripeService _stripeService;
        private readonly IMainArticleService _mainArticleService;

        public IdentityController(ILogger<IdentityController> logger, IAuthService authenticationService,
            IUserProfileService userProfileService, IStripeService stripeService,
            IMainArticleService mainArticleService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _userProfileService = userProfileService;
            _stripeService = stripeService;
            _mainArticleService = mainArticleService;
        }

        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login()
        {
           
            if (UserRoleHelper.IsUserRole(User.Claims, "User"))
            {
                return Redirect("~/");
            }
            return View();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(userLogin);
            }

            var response = await _authenticationService.Login(userLogin);
            if (response.IsSuccess)
            {
                var user = await _authenticationService.GetProfile(response.Data.UserNetUid);

                if (user.IsSuccess)
                {
                    Response.Cookies.Append(CookiesPath.USER_NAME, user.Data.UserName);
                    Response.Cookies.Append(CookiesPath.USER_ID, user.Data.Id.ToString());
                    Response.Cookies.Append(CookiesPath.EMAIL, user.Data.Email);
                    if (user.Data.SurName != null)
                    {
                        Response.Cookies.Append(CookiesPath.SURNAME, user.Data.SurName);

                    }
                    if (user.Data.InformationAccount != null)
                        Response.Cookies.Append(CookiesPath.INFORMATION_PROFILE, user.Data.InformationAccount);
                }
                await SignIn(response.Data);

                return Redirect("/");
            }
            else
            {
                await SetErrorMessage(response.Error.Message);
                return View(userLogin);
            }
        }

        [HttpGet]
        [Route("CreateAccount")]
        public async Task<IActionResult> CreateAccount()
        {
            if (UserRoleHelper.IsUserRole(User.Claims, "User"))
            {
                return Redirect("~/");
            }
            RegisterModel registerModel = new RegisterModel { IsEmailConfirmed = false };
            return View(registerModel);
        }
        [HttpPost]
        [Route("CreateAccount")]
        public async Task<IActionResult> CreateAccount(RegisterModel registerModel)
        {
            if (UserRoleHelper.IsUserRole(User.Claims, "User"))
            {
                return Redirect("~/");
            }
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }
            var result = await _userProfileService.CreateAccount(registerModel);
            if (result.IsSuccess)
            {
                registerModel.IsEmailConfirmed = true;
                return View(registerModel);
            }
            else
            {
                await SetErrorMessage(result.Error.Message);
                return View(registerModel);
            }
        }


        [HttpGet]
        [Route("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation(string access_token, string userId)
        {
            if (UserRoleHelper.IsUserRole(User.Claims, "User"))
            {
                return Redirect("~/");
            }
            var resultSucceeded = await _userProfileService.EmailConformation(access_token, userId);
            if (resultSucceeded)
            {
                return View();
            }
            else
            {
                return View();
            }

        }
        [NonAction]
        private async Task SignIn(CompleteAccessToken response)
        {
            var token = GetTokenFormat(response.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", response.AccessToken));
            claims.Add(new Claim("Guid", response.UserNetUid.ToString()));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        [NonAction]
        private static JwtSecurityToken GetTokenFormat(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}
