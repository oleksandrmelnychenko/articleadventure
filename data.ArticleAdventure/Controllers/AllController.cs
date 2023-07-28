using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using Microsoft.AspNetCore.Mvc;
using service.ArticleAdventure.Services.Blog.Contracts;
using domain.ArticleAdventure.Entities;
using data.ArticleAdventure.Views.All;
using data.ArticleAdventure.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace data.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.All)]
    public class AllController : WebApiControllerBase
    {
        private readonly IBlogService _blogService;
        public AllController(IBlogService blogService,IResponseFactory responseFactory) : base(responseFactory)
        {
            _blogService = blogService;
        }
        [HttpGet]
        [Authorize]
        [AssignActionRoute(AllSegments.ALL_BLOG)]
        public async Task<IActionResult> All()
        {
            List<Blogs> blogs = await _blogService.GetAllBlogs();

            AllModel model = new AllModel { Blogs = blogs };
            return View(model);
        }
        [HttpPost]
        [AssignActionRoute(AllSegments.EDIT)]
        public async Task<IActionResult> All(Blogs Blogs)
        {
            try
            {
                await _blogService.Update(Blogs);

                List<Blogs> blogs = await _blogService.GetAllBlogs();
                AllModel model = new AllModel { Blogs = blogs };
                return View(model);
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [Authorize]
        [AssignActionRoute(AllSegments.REMOVE_BLOG)]
        public async Task<IActionResult> All(Guid netUidBlog)
        {
            try
            {
                await _blogService.Remove(netUidBlog);

                List<Blogs> blogs = await _blogService.GetAllBlogs();
                AllModel model = new AllModel { Blogs = blogs };
                return View(model);
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
