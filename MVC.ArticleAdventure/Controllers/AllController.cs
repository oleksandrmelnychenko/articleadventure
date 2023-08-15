using common.ArticleAdventure.IdentityConfiguration;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.IdentityEntities;
using domain.ArticleAdventure.Models;
using domain.ArticleAdventure.Repositories;
using domain.ArticleAdventure.Repositories.Blog;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Services.Contract;
using System.Data;
using System.Security.Claims;

namespace MVC.ArticleAdventure.Controllers
{
    public class AllController : Controller
    {
        private readonly ILogger<AllController> _logger;
        private readonly IArticleService _authenticationService;
        public AllController(ILogger<AllController> logger, IArticleService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangeBlog(Guid netUidArticle)
        {
            var article = await _authenticationService.GetArticle(netUidArticle);
            ChangeArticleModel changeBlogModel = new ChangeArticleModel { Article = article };
            return View(changeBlogModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeBlog(ChangeArticleModel changeBlogModel)
        {
            await _authenticationService.Update(changeBlogModel.Article);
            return Redirect("~/All/AllBlogs");
        }

        [Authorize]
        public async Task<IActionResult> AllBlogs()
        {
            //if (!(User.FindFirstValue("role") == IdentityRoles.User))
            //{
            //    return Redirect("~/Login");
            //}
            var Articles = await _authenticationService.GetAllArticles();
            MainArticle AuthorArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, "mainArticle");
            var mainArticle = new List<MainArticle>();
            mainArticle.Add(AuthorArticle);
            AllArticlesModel model = new AllArticlesModel { Articles = Articles, mainArticles = mainArticle };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Remove(Guid netUidArticle)
        {
            await _authenticationService.Remove(netUidArticle);
            return Redirect("~/All/AllBlogs");
        }

        [HttpGet]
        [Route("InfoArticle")]
        public async Task<IActionResult> InfoArticle(Guid NetUidArticle)
        {
            MainArticle AuthorArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, "mainArticle");

            return View();
        }
    }
}
