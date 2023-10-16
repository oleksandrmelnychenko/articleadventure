using common.ArticleAdventure.WebApi.RoutingConfiguration.Maps;
using common.ArticleAdventure.WebApi;
using Microsoft.AspNetCore.Mvc;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using service.ArticleAdventure.Services.Stripe.Contracts;
using domain.ArticleAdventure.Entities;
using System.Net;
using domain.ArticleAdventure.Helpers;
using common.ArticleAdventure.Helpers;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        [AssignActionRoute(StripeSegments.CHECKOUT_BUY_NOW_MAIN_ARTICLE)]
        public async Task<IActionResult> CheckoutOrderBuyNowMainArticle( string emailUser,[FromBody] MainArticle mainArticle)
        {
            try
            {
                return Ok(SuccessResponseBody(await _stripeService.CheckOutBuyNowMainArticle(mainArticle, emailUser), ControllerMessageConstants.StripeMessage.CheckoutOrderBuyNowMainArticle)); 
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpPost]
        [Authorize]
        [AssignActionRoute(StripeSegments.CHECKOUT_BUY_NOW_SUP_ARTICLE)]
        public async Task<IActionResult> CheckoutOrderBuyNowSupArticle(string emailUser, [FromBody] AuthorArticle supArticle)
        {
            try
            {
                return Ok(SuccessResponseBody(await _stripeService.CheckOutBuyNowSupArticle(supArticle, emailUser), ControllerMessageConstants.StripeMessage.CheckoutOrderBuyNowSupArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }


        [HttpPost]
        [Authorize]
        [AssignActionRoute(StripeSegments.CHECKOUT_BUY_CART)]
        public async Task<IActionResult> CheckoutOrderBuyCartMain(string emailUser, [FromBody] List<MainArticle> mainArticle)
        {
            try
            {
                return Ok(SuccessResponseBody(await _stripeService.CheckOutBuyCartMainArticle(mainArticle, emailUser), ControllerMessageConstants.StripeMessage.CheckoutOrderBuyCartMain));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(StripeSegments.CHECKOUT_SUCCESS_MAIN_ARTICLE)]
        public async Task<IActionResult> CheckoutSuccess(string sessionId)
        {
            try
            {
                await _stripeService.CheckoutSuccessMainArticle(sessionId);
                return Ok(SuccessResponseBody(null, ControllerMessageConstants.StripeMessage.CheckoutSuccessMainArticle)); 
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [AssignActionRoute(StripeSegments.CHECKOUT_SUCCESS_SUP_ARTICLE)]
        public async Task<IActionResult> CheckoutSuccessSup(string sessionId)
        {
            try
            {
                await _stripeService.CheckoutSuccessSupArticle(sessionId);
                return Ok(SuccessResponseBody(null, ControllerMessageConstants.StripeMessage.CheckoutSuccessSupArticle));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
        [HttpGet]
        [AssignActionRoute(StripeSegments.CHECKOUT_SUCCESS_CART_ARTICLE)]
        public async Task<IActionResult> CheckoutSuccessCart(string sessionId)
        {
            try
            {
                await _stripeService.CheckoutSuccessCartArticle(sessionId);
                return Ok(SuccessResponseBody(null, ControllerMessageConstants.StripeMessage.CheckoutSuccessCart));
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
                return Ok(SuccessResponseBody(null, ControllerMessageConstants.StripeMessage.CheckoutFailed));
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
                return Ok(SuccessResponseBody(payments, ControllerMessageConstants.StripeMessage.CheckPaymentsHaveUser));
            }
            catch (Exception exc)
            {
                Logger.Log(NLog.LogLevel.Error, exc.Message);
                return BadRequest(ErrorResponseBody(exc.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
