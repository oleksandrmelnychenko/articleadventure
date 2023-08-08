using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Services.Contract;
using Microsoft.AspNetCore.Authorization;

namespace MVC.ArticleAdventure.Controllers
{
    public class NewController: Controller
    {
        private readonly ILogger<NewController> _logger;
        private readonly IArticleService _authenticationService;

        public NewController(ILogger<NewController> logger, IArticleService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }
        [HttpGet]
        [Authorize]
        [Route("NewBlog")]
        public async Task<IActionResult> NewBlog()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [Route("NewBlog")]
        public async Task<IActionResult> NewBlog(NewBlogModel newBlogModel)
        {
            AuthorArticle sendBlog = new AuthorArticle { Body = newBlogModel.Body, Title = newBlogModel.Title , Description= newBlogModel.Description };
             await _authenticationService.AddArticle(sendBlog);
            return Redirect("~/All/AllBlogs");
        }
        [HttpGet]
        [Route("LogOutAccount")]
        public async Task<IActionResult> LogOutAccount()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/Login");
        }
    }
}
