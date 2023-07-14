using common.ArticleAdventure.ResponceBuilder.Contracts;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using Microsoft.AspNetCore.Mvc;
using service.ArticleAdventure.Services.Blog.Contracts;
using System.Net;

namespace data.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Blog)]
    public class BlogController : WebApiControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogController(IResponseFactory responseFactory,
            IBlogService blogService) : base(responseFactory)
        {
            _blogService = blogService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AssignActionRoute(BlogSegments.GET)]
        public async Task<IActionResult> CreateUserProfileAsync()
        {
            try
            {
                return Ok(SuccessResponseBody(await _blogService.GetTestBlog(), "New blog successfully created"));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
