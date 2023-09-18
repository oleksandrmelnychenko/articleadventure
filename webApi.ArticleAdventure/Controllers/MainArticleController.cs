using common.ArticleAdventure.Helpers;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using service.ArticleAdventure.Services.Blog.Contracts;
using System.Net;

namespace webApi.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Article)]
    public class MainArticleController : WebApiControllerBase
    {
        private readonly IMainArticleService _mainArticleService;
        public MainArticleController(IResponseFactory responseFactory
            , IMainArticleService mainArticleService) : base(responseFactory)
        {
            _mainArticleService = mainArticleService;
        }
        [HttpPost]
        [AssignActionRoute(ArticleSegments.UPDATE)]
        public async Task<IActionResult> Update([FromForm] string article, [FromForm] IFormFile filePhotoMainArticle)
        {
            try
            {
                MainArticle? articleDeserialize = JsonConvert.DeserializeObject<MainArticle>(article);
                return Ok(SuccessResponseBody(await _mainArticleService.Update(articleDeserialize, filePhotoMainArticle), ControllerMessageConstants.ArticlesMessage.UpdateArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(ArticleSegments.ADD)]
        public async Task<IActionResult> AddArticle([FromForm] string article, [FromForm] IFormFile filePhotoMainArticle)
        {
            try
            {
                MainArticle? articleDeserialize = JsonConvert.DeserializeObject<MainArticle>(article);
                return Ok(SuccessResponseBody(await _mainArticleService.AddArticle(articleDeserialize, filePhotoMainArticle), ControllerMessageConstants.ArticlesMessage.AddMainArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }



        [HttpGet]
        [AssignActionRoute(ArticleSegments.ALL_ARTICLE_FILTERED_DATE_TIME)]
        public async Task<IActionResult> GetAllFilterDateTimeArticles()
        {
            try
            {
                return Ok(SuccessResponseBody(await _mainArticleService.GetAllFilterDateTimeArticles(), ControllerMessageConstants.ArticlesMessage.GetAllFilterDateTimeArticles));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(ArticleSegments.ALL_ARTICLE)]
        public async Task<IActionResult> All()
        {
            try
            {
                return Ok(SuccessResponseBody(await _mainArticleService.GetAllArticles(), ControllerMessageConstants.ArticlesMessage.GetAllArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(ArticleSegments.GET_ALL_ARTICLE_FILTER_SUP_TAGS)]
        public async Task<IActionResult> GetAllArticleFilterSupTags([FromBody] List<MainArticleTags> mainArticleTags)
        {
            try
            {
                return Ok(SuccessResponseBody(await _mainArticleService.GetAllArticlesFilterSupTags(mainArticleTags), ControllerMessageConstants.ArticlesMessage.AllArticleFilterSupTags));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(ArticleSegments.REMOVE_ARTICLE)]
        public async Task<IActionResult> EditArticle(Guid netUidArticle)
        {
            try
            {
                await _mainArticleService.Remove(netUidArticle);
                return Ok(SuccessResponseBody(null, ControllerMessageConstants.ArticlesMessage.RemoveArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(ArticleSegments.GET_ARTICLE)]
        public async Task<IActionResult> GetArticle(Guid netUidArticle)
        {
            try
            {
                var article = await _mainArticleService.GetArticle(netUidArticle);
                return Ok(SuccessResponseBody(article, ControllerMessageConstants.ArticlesMessage.GetMainArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(ArticleSegments.GET_SUP_ARTICLE)]
        public async Task<IActionResult> GetSupArticle(Guid netUidArticle)
        {
            try
            {
                var article = await _mainArticleService.GetSupArticle(netUidArticle);
                return Ok(SuccessResponseBody(article, ControllerMessageConstants.ArticlesMessage.GetSupArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }



        [HttpGet]
        [AssignActionRoute(ArticleSegments.GET_USER_ALL_STRIPE_PAYMENTS)]
        public async Task<IActionResult> GetAllPaymentArticleUser(long idUser)
        {
            try
            {
                return Ok(SuccessResponseBody(await _mainArticleService.GetAllPaymentArticleUser(idUser), ControllerMessageConstants.ArticlesMessage.GetUserStripePayments));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(ArticleSegments.GET_USER_ALL_ARTICLES)]
        public async Task<IActionResult> GetAllUserArticle(long idUser)
        {
            try
            {
                return Ok(SuccessResponseBody(await _mainArticleService.GetAllArticlesUser(idUser), ControllerMessageConstants.ArticlesMessage.GetUserAllArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(ArticleSegments.GET_USER_ARTICLE)]
        public async Task<IActionResult> GetUserArticle(Guid netUidArticle, long idUser)
        {
            try
            {
                return Ok(SuccessResponseBody(await _mainArticleService.GetArticleUser(netUidArticle, idUser), ControllerMessageConstants.ArticlesMessage.GetUserMainArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
