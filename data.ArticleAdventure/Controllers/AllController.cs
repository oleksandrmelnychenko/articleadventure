using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using Microsoft.AspNetCore.Mvc;
using service.ArticleAdventure.Services.Blog.Contracts;
using domain.ArticleAdventure.Entities;
using data.ArticleAdventure.Views.All;

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

        public async Task<IActionResult> All()
        {
            List<Blogs> blogs = await _blogService.GetAllBlogs();

            AllModel model = new AllModel { Blogs = blogs };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> All(AllModel models)
        {
            List<Blogs> blogs = await _blogService.GetAllBlogs();

            AllModel model = new AllModel { Blogs = blogs };
            return View(model);
        }

    }
}
