using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using Microsoft.AspNetCore.Mvc;
using service.ArticleAdventure.Services.Blog.Contracts;
using System.Net;
using data.ArticleAdventure.Models;
using domain.ArticleAdventure.Entities;

namespace data.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.New)]
    public class NewController : WebApiControllerBase
    {
        private readonly IBlogService _blogService;
        public NewController(IResponseFactory responseFactory,
            IBlogService blogService) : base(responseFactory)
        {
            _blogService = blogService;
        }

        [AssignActionRoute(NewSegments.NEW)]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [AssignActionRoute(NewSegments.NEW)]
        public async Task<IActionResult> New(Blogs newBlogModel)
        {
            try
            {
                await _blogService.AddBlog(newBlogModel);
                return View();
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            } 
        }

    }
}
