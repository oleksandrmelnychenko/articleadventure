using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using Microsoft.AspNetCore.Mvc;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using service.ArticleAdventure.Services.Tag.Contracts;
using domain.ArticleAdventure.Entities;
using System.Net;
using common.ArticleAdventure.Helpers;
using Azure;

namespace webApi.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Tags)]
    public class TagController : WebApiControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(IResponseFactory responseFactory,ITagService tagService) : base(responseFactory)
        {
            _tagService = tagService;
        }

        [HttpPost]
        [AssignActionRoute(TagSegments.CHANGE_MAIN_TAG)]
        public async Task<IActionResult> UpdateMainTag([FromBody] MainTag mainTag)
        {
            try
            {
                await _tagService.ChangeMainTag(mainTag);
                return Ok(SuccessResponseBody(null, ControllerMessageConstants.TagMessage.UpdateTag));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpPost]
        [AssignActionRoute(TagSegments.CHANGE_SUP_TAG)]
        public async Task<IActionResult> UpdateSubTag([FromBody] SupTag subTag)
        {
            try
            {
                await _tagService.ChangeSupTag(subTag);
                return Ok(SuccessResponseBody(null, ControllerMessageConstants.TagMessage.UpdateTag));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpPost]
        [AssignActionRoute(TagSegments.ADD_MAIN_TAG)]
        public async Task<IActionResult> AddMainTag([FromBody] MainTag mainTag)
        {
            try
            {
                return Ok(SuccessResponseBody(await _tagService.AddMainTag(mainTag), ControllerMessageConstants.TagMessage.AddTag));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(TagSegments.ADD_SUP_TAG)]
        public async Task<IActionResult> AddSubTag([FromBody] SupTag supTag)
        {
            try
            {
                return Ok(SuccessResponseBody(await _tagService.AddTag(supTag), ControllerMessageConstants.TagMessage.AddTag));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(TagSegments.ALL_MAIN_TAG)]
        public async Task<IActionResult> AllMainTag()
        {
            try
            {
                var allMainTag = await _tagService.AllMainTag();
                return Ok(SuccessResponseBody(allMainTag, ControllerMessageConstants.TagMessage.AllMainTag));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(TagSegments.ALL_SUP_TAG)]
        public async Task<IActionResult> AllSubTag()
        {
            try
            {
                var allMainTag = await _tagService.AllTag();
                return Ok(SuccessResponseBody(allMainTag, ControllerMessageConstants.TagMessage.AllSupTag));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        

        [HttpGet]
        [AssignActionRoute(TagSegments.GET_SUP_TAG)]
        public async Task<IActionResult> GetSupTag(Guid netUidSupTag)
        {
            try
            {
                return Ok(SuccessResponseBody(await _tagService.GetSupTag(netUidSupTag), ControllerMessageConstants.TagMessage.GetTag));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(TagSegments.GET_MAIN_TAG)]
        public async Task<IActionResult> GetMainTag(Guid netUidMainTag)
        {
            try
            {
                return Ok(SuccessResponseBody(await _tagService.GetMainTag(netUidMainTag), ControllerMessageConstants.TagMessage.GetTag));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(TagSegments.REMOVE_MAIN_TAG)]
        public async Task<IActionResult> RemoveMainTag(Guid netUidMainTag)
        {
            try
            {
                await _tagService.RemoveMainTag(netUidMainTag);
                return Ok(SuccessResponseBody(null, ControllerMessageConstants.TagMessage.RemoveTag));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(TagSegments.REMOVE_SUP_TAG)]
        public async Task<IActionResult> RemoveSubTag(Guid netUidSupTag)
        {
            try
            {
                await _tagService.RemoveTag(netUidSupTag);
                return Ok(SuccessResponseBody(null, ControllerMessageConstants.TagMessage.RemoveTag));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
