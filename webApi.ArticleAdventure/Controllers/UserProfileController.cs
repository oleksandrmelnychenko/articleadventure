using common.ArticleAdventure.ResponceBuilder.Contracts;
using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Mvc;
using service.ArticleAdventure.Services.UserManagement.Contracts;
using System.Net;

namespace webApi.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.UserManagement)]
    public class UserProfileController : WebApiControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        public UserProfileController(
          IUserProfileService userProfileService,
          IResponseFactory responseFactory) : base(responseFactory)
        {
            _userProfileService = userProfileService;
        }

        [HttpPost]
        [AssignActionRoute(UserManagementSegments.CREATE_USER)]
        public async Task<IActionResult> CreateUserProfileAsync([FromBody] UserProfile userProfile, [FromQuery] string password)
        {
            try
            {
                return Ok(SuccessResponseBody(await _userProfileService.Create(userProfile, password), "New user successfully created"));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(UserManagementSegments.FULL_UPDATE_USER_PROFILE)]
        public async Task<IActionResult> FullUpdateUserProfileAsync([FromBody] UserProfile userProfile, [FromQuery] string password)
        {
            try
            {
                return Ok(
                    SuccessResponseBody(
                        await _userProfileService.FullUpdate(
                            userProfile,
                            password
                        ),
                        "User successfully updated")
                );
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(UserManagementSegments.EMAIL_CONFORMATION)]
        public async Task<IActionResult> EmailConformation(string token, string userId)
        {
            try
            {
                
                return Ok(SuccessResponseBody(await _userProfileService.ConforimEmail(token, userId), "New user successfully updated Email"));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(UserManagementSegments.GET_USER_NETUID)]
        public async Task<IActionResult> GetNetUidUser(Guid userNetId)
        {
            try
            {

                return Ok(SuccessResponseBody(await _userProfileService.GetById(userNetId), "New user successfully updated Email"));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

    }
}
