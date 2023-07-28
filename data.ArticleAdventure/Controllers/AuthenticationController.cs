using common.ArticleAdventure.ResponceBuilder.Contracts;
using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using domain.ArticleAdventure.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.ArticleAdventure.Services.UserManagement.Contracts;
using System.Net;
using System.Security.Claims;
using data.ArticleAdventure.Models;
using data.ArticleAdventure.Views.Authentication;
using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json.Linq;
using static Dapper.SqlMapper;

namespace data.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Authentication)]
    public class AuthenticationController : WebApiControllerBase
    {
        private readonly IRequestTokenService _requestTokenService;
        private readonly IUserProfileService _userProfileService;

        public AuthenticationController(
            IRequestTokenService requestTokenService,
            IUserProfileService userProfileService,
            IResponseFactory responseFactory) : base(responseFactory)
        {
            _requestTokenService = requestTokenService;
            _userProfileService = userProfileService;
        }

        public async Task<IActionResult> Login()
        {
            LoginModel loginModel = new LoginModel
            {
                LoginModels = new LoginModels { /*Msg = string.Empty, Password = string.Empty, Username = string.Empty*/ }
            };
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModels LoginModels)
        {
            try
            {
                domain.ArticleAdventure.EntityHelpers.Identity.CompleteAccessToken token = await _requestTokenService.RequestToken(LoginModels.Email, LoginModels.Password, false);
                if (!string.IsNullOrEmpty(token.RefreshToken))
                {
                    Response.Cookies.Append("access_token", token.AccessToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Set to true if using HTTPS
                        MaxAge = TimeSpan.FromDays(7) // Adjust expiration time as needed
                    });

                    Response.Cookies.Append("refresh_token", token.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Set to true if using HTTPS
                        MaxAge = TimeSpan.FromDays(7) // Adjust expiration time as needed
                    });
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, LoginModels.Email)
                        };

                    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                    var a = (User.Identity.IsAuthenticated);

                }
                return View();
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        //[HttpGet]
        //[AssignActionRoute(UserManagementSegments.REQUEST_TOKEN)]
        //public async Task<IActionResult> RequestTokenAsync(string email, string password, bool rememberUser)
        //{
        //    try
        //    {
        //        return Ok(SuccessResponseBody(await _requestTokenService.RequestToken(email, password, rememberUser)));
        //    }
        //    catch (Exception exc)
        //    {
        //        Logger.Log(NLog.LogLevel.Error, exc.Message);
        //        return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
        //    }
        //}

        //[HttpGet]
        //[AssignActionRoute(UserManagementSegments.REFRESH_TOKEN)]
        //public async Task<IActionResult> RefreshTokenAsync(string refreshToken)
        //{
        //    try
        //    {
        //        return Ok(SuccessResponseBody(await _requestTokenService.RefreshToken(refreshToken)));
        //    }
        //    catch (Exception exc)
        //    {
        //        Logger.Log(NLog.LogLevel.Error, exc.Message);
        //        return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
        //    }
        //}

        //[HttpGet]
        //[Authorize]
        //[AssignActionRoute(UserManagementSegments.DELETE_TOKEN)]
        //public async Task<IActionResult> DeleteRefreshTokenOnLogoutAsync()
        //{
        //    try
        //    {
        //        Claim userNetIdClaim = User.Claims.FirstOrDefault(c => c.Type.Equals("NetId"));

        //        if (userNetIdClaim == null) return Unauthorized();

        //        await _requestTokenService.DeleteRefreshTokenOnLogoutByUserId(Guid.Parse(userNetIdClaim.Value));

        //        return Ok(SuccessResponseBody(null));
        //    }
        //    catch (Exception exc)
        //    {
        //        Logger.Log(NLog.LogLevel.Error, exc.Message);
        //        return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
        //    }
        //}
    }
}
