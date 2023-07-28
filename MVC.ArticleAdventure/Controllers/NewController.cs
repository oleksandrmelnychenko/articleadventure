using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Services.Contract;

namespace MVC.ArticleAdventure.Controllers
{
    public class NewController: Controller
    {
        private readonly ILogger<AllController> _logger;
        private readonly IArticleService _authenticationService;

        public NewController(ILogger<AllController> logger, IArticleService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }
        [HttpGet]
        [Route("NewBlog")]
        public async Task<IActionResult> NewBlog()
        {
            return View();
        }
        [HttpPost]
        [Route("NewBlog")]
        public async Task<IActionResult> NewBlog(NewBlogModel newBlogModel)
        {
            AuthorArticle sendBlog = new AuthorArticle { Body = newBlogModel.Body, Title = newBlogModel.Title , Description= newBlogModel.Description };
             await _authenticationService.AddArticle(sendBlog);
            return Redirect("~/All/AllBlogs");
        }
    }
}
