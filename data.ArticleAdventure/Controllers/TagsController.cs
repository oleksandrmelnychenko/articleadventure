using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace data.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Tags)]
    public class TagsController : WebApiControllerBase
    {
        public TagsController(IResponseFactory responseFactory) : base(responseFactory)
        {
        }
 
        public IActionResult Tags()
        {
            return View();
        }

    }
}
