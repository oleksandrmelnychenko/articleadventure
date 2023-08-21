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
        IStripeService _stripeService;
        public StripeController(IResponseFactory responseFactory,IStripeService stripeService) : base(responseFactory)
        {
            _stripeService = stripeService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AssignActionRoute(StripeSegments.ADD_STRIPE_CUSTOMER)]
        public async Task<IActionResult> AddStripeCustomerAsync([FromBody] AddStripeCustomer customer,
            CancellationToken ct)
        {
            try
            {
               var customerResponce = await _stripeService.AddStripeCustomerAsync(customer,ct);
                return Ok(SuccessResponseBody(customerResponce, "Статус успешно изменён."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        [AssignActionRoute(StripeSegments.ADD_STRIPE_PAYMENT)]
        public async Task<IActionResult> AddStripePayment([FromBody] AddStripePayment payment,
           CancellationToken ct)
        {
            try
            {
                var paymentResponce = await _stripeService.AddStripePaymentAsync(payment, ct);
                return Ok(SuccessResponseBody(paymentResponce, "Статус успешно изменён."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
