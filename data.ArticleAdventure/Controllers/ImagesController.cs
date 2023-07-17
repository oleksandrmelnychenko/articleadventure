using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using Microsoft.AspNetCore.Mvc;
using common.ArticleAdventure.ResponceBuilder.Contracts;

namespace data.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Images)]
    public class ImagesController : WebApiControllerBase
    {
        public ImagesController(IResponseFactory responseFactory) : base(responseFactory)
        {
        }

        public IActionResult Images()
        {
            return View();
        }
    }
}
