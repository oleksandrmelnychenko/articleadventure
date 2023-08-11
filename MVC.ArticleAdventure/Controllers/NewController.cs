using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace MVC.ArticleAdventure.Controllers
{
    public class NewController : Controller
    {
        private readonly ILogger<NewController> _logger;
        private readonly IArticleService _authenticationService;
        private readonly ITagService _tagService;


        public NewController(ILogger<NewController> logger, IArticleService authenticationService, ITagService tagService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _tagService = tagService;
        }
        [HttpGet]
        [Authorize]
        [Route("NewBlog")]
        public async Task<IActionResult> NewBlog()
        {
            var mainTags = await _tagService.GetAllTags();
            NewBlogModel newBlogModel = new NewBlogModel { MainTags = mainTags };
            return View(newBlogModel);
        }
        [HttpPost]
        [Authorize]
        [Route("NewBlog")]
        public async Task<IActionResult> NewBlog(NewBlogModel newBlogModel)
        {
            AuthorArticle sendBlog = new AuthorArticle { Body = newBlogModel.Body, Title = newBlogModel.Title, Description = newBlogModel.Description };
            await _authenticationService.AddArticle(sendBlog);
            return Redirect("~/All/AllBlogs");
        }

        [HttpPost]
        [Route("IsSelect")]
        public async Task<IActionResult> IsSelect(NewBlogModel newBlogModel)
        {
            var mainTags = await _tagService.GetAllTags();
            var findSupTag = mainTags
            .SelectMany(mainTag => mainTag.SubTags)
            .FirstOrDefault(subTag => subTag.NetUid == newBlogModel.NetUidTags);
            newBlogModel.SelectSupTags.Add(findSupTag);
            newBlogModel.MainTags = mainTags;
            //NewBlogModel newBlogModel = new NewBlogModel { IsTags = true };
            return View("NewBlog", newBlogModel);
        }

        [HttpGet]
        public async Task<IActionResult> SeeTags()
        {

            return View("NewBlog");
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
