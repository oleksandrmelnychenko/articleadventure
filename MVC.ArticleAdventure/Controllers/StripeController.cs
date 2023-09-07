using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
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



            //SessionExtensionsMVC.Set(HttpContext.Session, "article", article);
            if (listAutorArticle == null)
            {
                //basketModel.BasketArticles = new List<AuthorArticle>();
            }
            return View(basketModel);
        }

        [HttpGet]
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
        public async Task<IActionResult> SetFavoriteArticle(Guid netUidArticle)
        {
            var article = await _mainArticleService.GetArticle(netUidArticle);
            var listAuthorArticle = SessionExtensionsMVC.Get<List<MainArticle>>(HttpContext.Session, SessionStoragePath.CART_ARTICLE);
            if (listAuthorArticle == null)
            {
                List<MainArticle> authorArticle = new List<MainArticle>();
                authorArticle.Add(article);
                SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CART_ARTICLE, authorArticle);

                return Redirect("~/All/AllBlogs");
            }
            else
            {
                listAuthorArticle.Add(article);
                SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CART_ARTICLE, listAuthorArticle);
            }

            return Redirect("~/All/AllBlogs");
        }

        [HttpGet]
        public async Task<IActionResult> BuyNow(Guid netUidBuyArticle)
        {
            var article = await _mainArticleService.GetArticle(netUidBuyArticle);
            
            var email = Request.Cookies[CookiesPath.EMAIL];


            var orderInfo = await _stripeService.BuyStripe(article, email);
            BuyNowModel buyNowModel = new BuyNowModel { orderResponse = orderInfo };
            return View(buyNowModel);
        }

        [HttpGet]
        public async Task<IActionResult> BuySup(Guid netUidBuyArticle)
        {
            var article = await _mainArticleService.GetArticle(netUidBuyArticle);

            var email = Request.Cookies[CookiesPath.EMAIL];


            var orderInfo = await _stripeService.BuyStripe(article, email);
            BuyNowModel buyNowModel = new BuyNowModel { orderResponse = orderInfo };
            return View(buyNowModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> SuccessBuy(string sessionId)
        {
             await _stripeService.CheckoutSuccess(sessionId);

            return View();
        }
        
    }
}
