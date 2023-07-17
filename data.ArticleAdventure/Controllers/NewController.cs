using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace data.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.New)]
    public class NewController : WebApiControllerBase
    {
        public NewController(IResponseFactory responseFactory) : base(responseFactory)
        {

        }
        public IActionResult New()
        {
            return View();
        }

    }
}
