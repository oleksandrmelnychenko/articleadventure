using common.ArticleAdventure.ResponceBuilder.Contracts;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Update([FromBody] MainArticle article)
        {
            try
            {
                await _mainArticleService.Update(article);
                return Ok(SuccessResponseBody(null, "Статус успешно изменён."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(ArticleSegments.ADD)]
        public async Task<IActionResult> AddArticle([FromBody] MainArticle article)
        {
            try
            {
                await _mainArticleService.AddArticle(article);
                return Ok(SuccessResponseBody(null, "Статус успешно изменён."));
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
                List<MainArticle> articles = await _mainArticleService.GetAllArticles();
                return Ok(SuccessResponseBody(articles, "Успешно вытянуты данные."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(ArticleSegments.REMOVE_ARTICLE)]
        public async Task<IActionResult> EditArticle (Guid netUidArticle)
        {
            try
            {
                await _mainArticleService.Remove(netUidArticle);
                return Ok(SuccessResponseBody(null, "Успешно удаленые данные."));
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
                return Ok(SuccessResponseBody(article, "Успешно вытянуты данные."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
