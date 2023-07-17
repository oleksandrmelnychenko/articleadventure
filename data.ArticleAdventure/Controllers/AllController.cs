using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace data.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.All)]
    public class AllController : WebApiControllerBase
    {
        public AllController(IResponseFactory responseFactory) : base(responseFactory)
        {
        }

        public IActionResult All()
        {
            return View();
        }
       
    }
}
