using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Services.Contract;
using System.Collections.Generic;

namespace MVC.ArticleAdventure.Controllers
{
    public class StripeController : Controller
    {
        private readonly ILogger<StripeController> _logger;
        private readonly IArticleService _authenticationService;

        public StripeController(ILogger<StripeController> logger,IArticleService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public async Task<IActionResult> Basket()
        {
            var listAutorArticle = SessionExtensionsMVC.Get<List<AuthorArticle>>(HttpContext.Session, "article");
            BasketModel basketModel = new BasketModel { BasketArticles = listAutorArticle };

            //SessionExtensionsMVC.Set(HttpContext.Session, "article", article);
            if (listAutorArticle== null)
            {
                //basketModel.BasketArticles = new List<AuthorArticle>();
            }
            return View(basketModel);
        }
        [HttpGet]
        public async Task<IActionResult> SetFavoriteArticle(Guid netUidArticle)
        {
            var article = await _authenticationService.GetArticle(netUidArticle);
            var listAuthorArticle = SessionExtensionsMVC.Get<List<AuthorArticle>>(HttpContext.Session, "article");
            if (listAuthorArticle == null)
            {
                List<AuthorArticle> authorArticle = new List<AuthorArticle>();
                authorArticle.Add(article);
                SessionExtensionsMVC.Set(HttpContext.Session, "article", authorArticle);

                return Redirect("~/All/AllBlogs"); 
            }
            else
            {
                listAuthorArticle.Add(article);
                SessionExtensionsMVC.Set(HttpContext.Session, "article", listAuthorArticle);

            }

            return Redirect("~/All/AllBlogs");
        }

       
    }
}
