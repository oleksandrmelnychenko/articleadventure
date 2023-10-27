using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MVC.ArticleAdventure.Controllers
{
    public class StripeController : Controller
    {
        private readonly ILogger<StripeController> _logger;
        private readonly IArticleService _authenticationService;
        private readonly IMainArticleService _mainArticleService;
        private readonly IStripeService _stripeService;

        public StripeController(ILogger<StripeController> logger, IArticleService authenticationService, IMainArticleService mainArticleService, IStripeService stripeService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _mainArticleService = mainArticleService;
            _stripeService = stripeService;
        }

        [HttpGet]
        public async Task<IActionResult> Basket()
        {
            var listAutorArticle = SessionExtensionsMVC.Get<List<MainArticle>>(HttpContext.Session, SessionStoragePath.CART_ARTICLE);
            BasketModel basketModel = new BasketModel { BasketArticles = listAutorArticle };

            if (listAutorArticle != null)
            {
                var fullPrice = listAutorArticle.Select(x => x.Price).Sum();
                basketModel.FullPrice = fullPrice;
            }
            return View(basketModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> StripeStatistics()
        {
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            ExecutionResult<List<StripePayment>> payments = await _stripeService.GetStripePayments(token);
            ExecutionResult<List<StripeCustomer>> customers = await _stripeService.GetStripeCustomers(token);
            StripeStatistics stripeStatistics = new StripeStatistics { stripePayments = payments.Data, stripeCustomer = customers.Data };
            return View(stripeStatistics);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Basket(BasketModel basketModel)
        {
            var email = Request.Cookies[CookiesPath.EMAIL];
            var listAutorArticle = SessionExtensionsMVC.Get<List<MainArticle>>(HttpContext.Session, SessionStoragePath.CART_ARTICLE);
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            var result = await _stripeService.BuyStripeCartArticle(listAutorArticle, email, token);
            BuyNowModel buyNowModel = new BuyNowModel { orderResponse = result.Data };
            return View("BuyNow", buyNowModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RemoveFavoriteArticle(Guid netUidArticle)
        {
            var listAuthorArticle = SessionExtensionsMVC.Get<List<MainArticle>>(HttpContext.Session, SessionStoragePath.CART_ARTICLE);

            var removeArticle = listAuthorArticle.First(x => x.NetUid == netUidArticle);

            listAuthorArticle.Remove(removeArticle);
            if (listAuthorArticle.Count() == 0)
            {
                HttpContext.Session.Clear();
            }
            else
            {
                SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CART_ARTICLE, listAuthorArticle);
            }

            return Redirect("~/Stripe/Basket");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SetСartArticle(Guid netUidArticle)
        {
            var article = await _mainArticleService.GetArticle(netUidArticle);
            var listAuthorArticle = SessionExtensionsMVC.Get<List<MainArticle>>(HttpContext.Session, SessionStoragePath.CART_ARTICLE);
            if (listAuthorArticle == null)
            {
                List<MainArticle> authorArticle = new List<MainArticle>();
                authorArticle.Add(article);
                SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CART_ARTICLE, authorArticle);

                return Redirect("~/");
            }
            else
            {
                listAuthorArticle.Add(article);
                SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CART_ARTICLE, listAuthorArticle);
            }

            return Redirect("~/");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BuyNow(Guid netUidBuyArticle)
        {
            var article = await _mainArticleService.GetArticle(netUidBuyArticle);
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            var email = Request.Cookies[CookiesPath.EMAIL];

            var orderInfo = await _stripeService.BuyStripeMainArticle(article, email, token);
            BuyNowModel buyNowModel = new BuyNowModel { orderResponse = orderInfo };
            return View(buyNowModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BuySup(Guid netUidBuyArticle)
        {
            var article = await _mainArticleService.GeSupArticle(netUidBuyArticle);
            var email = Request.Cookies[CookiesPath.EMAIL];
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            var orderInfo = await _stripeService.BuyStripeSupArticle(article.Data, email, token);
            BuyNowModel buyNowModel = new BuyNowModel { orderResponse = orderInfo.Data };
            return View("BuyNow", buyNowModel);
        }

        [HttpGet]
        public async Task<IActionResult> SuccessBuy(string sessionId)
        {
            await _stripeService.CheckoutSuccess(sessionId);
            HttpContext.Session.Remove(SessionStoragePath.CART_ARTICLE);

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SuccessBuySup(string sessionId)
        {
            await _stripeService.CheckoutSuccessSup(sessionId);
            HttpContext.Session.Remove(SessionStoragePath.CART_ARTICLE);

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SuccessBuyCart(string sessionId)
        {
            await _stripeService.CheckoutSuccessCart(sessionId);
            HttpContext.Session.Remove(SessionStoragePath.CART_ARTICLE);

            return View();
        }
    }
}
