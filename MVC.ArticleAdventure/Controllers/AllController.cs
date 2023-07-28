using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Services.Contract;

namespace MVC.ArticleAdventure.Controllers
{
    public class AllController : Controller
    {
        private readonly ILogger<AllController> _logger;
        private readonly IArticleService _authenticationService;

        public AllController(ILogger<AllController> logger,IArticleService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }
        [HttpGet]
        public async Task<IActionResult> ChangeBlog(Guid netUidArticle) 
        {
            var article =  await _authenticationService.GetArticle(netUidArticle);
            ChangeBlogModel changeBlogModel = new ChangeBlogModel {  Article = article};
            return View(changeBlogModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeBlog(ChangeBlogModel changeBlogModel)
        {
            await _authenticationService.Update(changeBlogModel.Article);
            return Redirect("~/All/AllBlogs");
        }
        public async Task<IActionResult> AllBlogs()
        {
            var Articles = await _authenticationService.GetAllArticles();
            AllArticlesModel model= new AllArticlesModel { Articles = Articles};
            return View(model);
        }
        public async Task<IActionResult> Remove(Guid netUidArticle)
        {
            await _authenticationService.Remove(netUidArticle);
            return Redirect("~/All/AllBlogs");
        }
    }
}
