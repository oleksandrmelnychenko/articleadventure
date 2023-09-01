using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using Microsoft.AspNetCore.Mvc;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using service.ArticleAdventure.Services.Stripe.Contracts;
using domain.ArticleAdventure.Entities;
using System.Net;
using domain.ArticleAdventure.Helpers;

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
        [AssignActionRoute(StripeSegments.CHECKOUT_BUY_NOW)]
        public async Task<IActionResult> CheckoutOrderBuyNowMain( string emailUser,[FromBody] MainArticle mainArticle)
        {
            try
            {
                //var customerResponce = await _stripeService.AddStripeCustomerAsync(customer,ct);
                var checkout = await _stripeService.CheckOutBuyNow(mainArticle, "", emailUser);
                return Ok(SuccessResponseBody(checkout, "Статус успешно изменён.")); 
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }


        [HttpPost]
        [AssignActionRoute(StripeSegments.CHECKOUT_BUY_CART)]
        public async Task<IActionResult> CheckoutOrderBuyCartMain(string emailUser, [FromBody] List<MainArticle> mainArticle)
        {
            try
            {
                return Ok(SuccessResponseBody(null, "Статус успешно изменён."));
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
        [HttpGet]
        [AssignActionRoute(StripeSegments.CHECKOUT_FAILED)]
        public async Task<IActionResult> CheckoutFailed()
        {
            try
            {
                //var customerResponce = await _stripeService.AddStripeCustomerAsync(customer,ct);
                return Ok(SuccessResponseBody(null, "Статус успешно изменён."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(StripeSegments.CHECK_PAYMENTS_HAVE_USER)]
        public async Task<IActionResult> CheckPaymentsHaveUser(string userMail)
        {
            try
            {
                var payments = await _stripeService.CheckPaymentsHaveUser(userMail);
                //var customerResponce = await _stripeService.AddStripeCustomerAsync(customer,ct);
                return Ok(SuccessResponseBody(payments, "Статус успешно изменён."));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
