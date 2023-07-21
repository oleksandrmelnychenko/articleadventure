using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using Microsoft.AspNetCore.Mvc;
using service.ArticleAdventure.Services.Blog.Contracts;
using System.Net;
using data.ArticleAdventure.Models;
using domain.ArticleAdventure.Entities;
using data.ArticleAdventure.Views.New;
using common.ArticleAdventure.Helpers;

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
        public async Task<IActionResult> New(Blogs Blogs)
        {
            try
            {
                await _blogService.AddBlog(Blogs);
                return Redirect($"{ConnectionStringNames.ConnectionString}api/v1/all/all");
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(NewSegments.GET_BLOG)]
        public async Task<IActionResult> EditBLog(Guid netUidBlog)
        {
            try
            {
                var blog = await _blogService.GetBLog(netUidBlog);

                EditBlogModel newModel = new EditBlogModel
                {
                    Blogs = blog
                };

                return View(newModel);
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpPost]
        [AssignActionRoute(NewSegments.EDIT_BLOG)]
        public async Task<IActionResult> EditBLog(EditModel editModel)
        {
            try
            {
                await _blogService.Update(editModel.Blogs);
                return Redirect($"{ConnectionStringNames.ConnectionString}/api/v1/all/all");
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

    }
}
