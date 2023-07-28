using common.ArticleAdventure.ResponceBuilder.Contracts;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.ArticleAdventure.Services.Blog;
using service.ArticleAdventure.Services.Blog.Contracts;
using System.Net;

namespace webApi.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Blog)]
    public class BlogController : WebApiControllerBase
    {
        private readonly IArticleService _articleService;
        public BlogController(IResponseFactory responseFactory
            , IArticleService articleService) : base(responseFactory)
        {
            _articleService = articleService;
        }

        [HttpPost]
        [AssignActionRoute(BlogSegments.UPDATE)]
        public async Task<IActionResult> Update([FromBody] AuthorArticle article)
        {
                try
                {
                     await _articleService.Update(article);
                    return Ok(SuccessResponseBody(null, "Статус успешно изменён."));
                }
                catch (Exception exc)
                {
                    Logger.Log(NLog.LogLevel.Error, exc.Message);
                    return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
                }
        }

        [HttpPost]
        [AssignActionRoute(BlogSegments.ADD)]
        public async Task<IActionResult> AddBLog([FromBody] AuthorArticle Blogs)
        {
            try
            {
                await _articleService.AddArticle(Blogs);
                return Ok(SuccessResponseBody(null, "Статус успешно изменён."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(BlogSegments.ALL_BLOG)]
        public async Task<IActionResult> All()
        {
            try
            {
                List<AuthorArticle> blogs = await _articleService.GetAllArticles();
                return Ok(SuccessResponseBody(blogs, "Успешно вытянуты данные."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(BlogSegments.REMOVE_BLOG)]
        public async Task<IActionResult> EditBlog(Guid netUidArticle)
        {
            try
            {
                await _articleService.Remove(netUidArticle);
                return Ok(SuccessResponseBody(null, "Успешно удаленые данные."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(BlogSegments.GET_BLOG)]
        public async Task<IActionResult> GetBLog(Guid netUidArticle)
        {
            try
            {
                var blog = await _articleService.GetArticle(netUidArticle);
                return Ok(SuccessResponseBody(blog, "Успешно вытянуты данные."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
