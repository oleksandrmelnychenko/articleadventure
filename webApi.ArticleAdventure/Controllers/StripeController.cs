using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using Microsoft.AspNetCore.Mvc;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using service.ArticleAdventure.Services.Stripe.Contracts;
using domain.ArticleAdventure.Entities;
using System.Net;

namespace webApi.ArticleAdventure.Controllers
{
    [AssignControllerRoute(WebApiEnvironment.Current, WebApiVersion.ApiVersion1, ApplicationSegments.Stripe)]
    public class StripeController : WebApiControllerBase
    {
        private readonly IStripeService _stripeService;
        public StripeController(IResponseFactory responseFactory,IStripeService stripeService) : base(responseFactory)
        {
            _stripeService = stripeService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AssignActionRoute(StripeSegments.CHECKOUT)]
        public async Task<IActionResult> CheckoutOrder([FromBody] MainArticle mainArticle)
        {
            try
            {
                //var customerResponce = await _stripeService.AddStripeCustomerAsync(customer,ct);
                var s = await _stripeService.CheckOut(mainArticle, "https://localhost:7192");
                return Ok(SuccessResponseBody(s, "Статус успешно изменён."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(StripeSegments.CHECKOUT_SUCCESS)]
        public async Task<IActionResult> CheckoutSuccess(string sessionId)
        {
            try
            {
                //var customerResponce = await _stripeService.AddStripeCustomerAsync(customer,ct);
                await _stripeService.CheckoutSuccess(sessionId);
                return Ok(SuccessResponseBody(null, "Статус успешно изменён."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
