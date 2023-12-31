﻿using common.ArticleAdventure.ResponceBuilder.Contracts;
using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using domain.ArticleAdventure.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.ArticleAdventure.Services.UserManagement.Contracts;
using System.Net;
using System.Security.Claims;

namespace data.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.UserManagement)]
    public class RequestTokenController : WebApiControllerBase
    {
        private readonly IRequestTokenService _requestTokenService;

        public RequestTokenController(
            IRequestTokenService requestTokenService,
            IResponseFactory responseFactory) : base(responseFactory)
        {
            _requestTokenService = requestTokenService;
        }


        [HttpGet]
        [AssignActionRoute(UserManagementSegments.REQUEST_TOKEN)]
        public async Task<IActionResult> RequestTokenAsync(string email, string password, bool rememberUser)
        {
            try
            {
                return Ok(SuccessResponseBody(await _requestTokenService.RequestToken(email, password, rememberUser)));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(UserManagementSegments.REFRESH_TOKEN)]
        public async Task<IActionResult> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                return Ok(SuccessResponseBody(await _requestTokenService.RefreshToken(refreshToken)));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [Authorize]
        [AssignActionRoute(UserManagementSegments.DELETE_TOKEN)]
        public async Task<IActionResult> DeleteRefreshTokenOnLogoutAsync()
        {
            try
            {
                Claim userNetIdClaim = User.Claims.FirstOrDefault(c => c.Type.Equals("NetId"));

                if (userNetIdClaim == null) return Unauthorized();

                await _requestTokenService.DeleteRefreshTokenOnLogoutByUserId(Guid.Parse(userNetIdClaim.Value));

                return Ok(SuccessResponseBody(null));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

    }

}
