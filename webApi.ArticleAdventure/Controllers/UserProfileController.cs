using common.ArticleAdventure.ResponceBuilder.Contracts;
using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Mvc;
using service.ArticleAdventure.Services.UserManagement.Contracts;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using common.ArticleAdventure.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

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
                return Ok(SuccessResponseBody(await _userProfileService.Create(userProfile, password), ControllerMessageConstants.UserMessage.CreateUserProfileAsync));
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
                       ControllerMessageConstants.UserMessage.FullUpdateUserProfileAsync)
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
                return Ok(SuccessResponseBody(await _userProfileService.ConforimEmail(token, userId), ControllerMessageConstants.UserMessage.EmailConformation));
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
                return Ok(SuccessResponseBody(await _userProfileService.GetById(userNetId), ControllerMessageConstants.UserMessage.GetNetUidUser));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        
        [HttpGet]
        [AssignActionRoute(UserManagementSegments.UPDATE_PASSWORD)]
        public async Task<IActionResult> UpdatePassword(Guid userProfileNetUid, string newPassword,string oldPassword)
        {
            try
            {
                return Ok(SuccessResponseBody(await _userProfileService.UpdatePassword(userProfileNetUid, newPassword, oldPassword), ControllerMessageConstants.UserMessage.UpdatePassword));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(UserManagementSegments.UPDATE_EMAIL)]
        public async Task<IActionResult> UpdateEmail(Guid userProfileNetUid, string newEmail, string password)
        {
            try
            {
                return Ok(SuccessResponseBody(await _userProfileService.UpdateEmail(userProfileNetUid, newEmail, password), ControllerMessageConstants.UserMessage.UpdateEmail));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpPost]
        [AssignActionRoute(UserManagementSegments.UPDATE_ACCOUNT_INFORMATION)]
        public async Task<IActionResult> UpdateAccountInformation( [FromForm] string userProfile, [FromForm] IFormFile photoUserProfile)
        {
            try
            {
                UserProfile? userProfileDeserialize = JsonConvert.DeserializeObject<UserProfile>(userProfile);

                return Ok(SuccessResponseBody(await _userProfileService.UpdateAccountInformation(userProfileDeserialize,photoUserProfile), ControllerMessageConstants.UserMessage.UpdateAccountInformation));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(UserManagementSegments.SET_FAVORITE_ARTICLE)]
        public async Task<IActionResult> SetFavoriteArticle(Guid netUidArticle,Guid netUidUser)
        {
            try
            {
                return Ok(SuccessResponseBody(await _userProfileService.SetFavoriteArticle(netUidArticle,netUidUser), ControllerMessageConstants.UserMessage.SetFavoriteArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(UserManagementSegments.GET_All_FAVORITE_ARTICLE)]
        public async Task<IActionResult> GetAllFavoriteArticle(Guid userProfileNetUid)
        {
            try
            {
                return Ok(SuccessResponseBody(await _userProfileService.GetAllFavoriteArticle(userProfileNetUid), ControllerMessageConstants.UserMessage.GetAllFavoriteArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(UserManagementSegments.GET_FAVORITE_ARTICLE)]
        public async Task<IActionResult> GetFavoriteArticle(Guid netUidArticle, Guid netUidUser)
        {
            try
            {
                return Ok(SuccessResponseBody(await _userProfileService.GetFavoriteArticle(netUidArticle, netUidUser), ControllerMessageConstants.UserMessage.GetFavoriteArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(UserManagementSegments.REMOVE_FAVORITE_ARTICLE)]
        public async Task<IActionResult> RemoveFavoriteArticle(Guid netUidFavoriteArticle)
        {
            try
            {
                return Ok(SuccessResponseBody(await _userProfileService.RemoveFavoriteArticle(netUidFavoriteArticle), ControllerMessageConstants.UserMessage.RemoveFavoriteArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
