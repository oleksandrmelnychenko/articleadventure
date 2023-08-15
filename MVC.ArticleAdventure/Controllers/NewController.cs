﻿using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using domain.ArticleAdventure.Helpers;

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
            NewArticleModel newBlogModel = new NewArticleModel { MainTags = mainTags };
            return View(newBlogModel);
        }
        [HttpPost]
        [Authorize]
        [Route("NewBlog")]
        public async Task<IActionResult> NewBlog(NewArticleModel newBlogModel)
        {
            var AuthorArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, "mainArticle");
            if (AuthorArticle == null)
            {
                return View(newBlogModel);
            }
            else
            {
                return Redirect("~/SettingMainArticle");
            }

            //AuthorArticle sendBlog = new AuthorArticle { Body = newBlogModel.Body, Title = newBlogModel.Title, Description = newBlogModel.Description };
            //await _authenticationService.AddArticle(sendBlog);
            //return Redirect("~/All/AllBlogs");
        }

        [HttpPost]
        [Route("IsSelect")]
        public async Task<IActionResult> IsSelect(NewArticleModel newBlogModel)
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

        [HttpPost]
        [Route("SubArticle")]
        public async Task<IActionResult> SubArticle(NewArticleModel newBlogModel)
        {
            AuthorArticle createSubArticle = new AuthorArticle { 
                Title = newBlogModel.authorArticle.Title,
                Body = newBlogModel.authorArticle.Body ,
                Description = newBlogModel.authorArticle.Description ,
                Price = newBlogModel.authorArticle.Price};

            NewArticleModel newArticleModel = new NewArticleModel { Title = newBlogModel.Title };

            //AuthorArticle createSubArticle = new AuthorArticle { Body = newBlogModel.Body, Title = newBlogModel.Title, Description = newBlogModel.Description };

            var AuthorArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, "mainArticle");
            if (AuthorArticle == null)
            {
                MainArticle mainArticle = new MainArticle { Title = newBlogModel.Title, Articles = new List<AuthorArticle>() };
                mainArticle.Articles.Add(createSubArticle);
                SessionExtensionsMVC.Set(HttpContext.Session, "mainArticle", mainArticle);

            }
            else
            {
                AuthorArticle.Articles.Add(createSubArticle);
                SessionExtensionsMVC.Set(HttpContext.Session, "mainArticle", AuthorArticle);

            }
            ModelState.Clear();
            return View(newArticleModel);
        }


        [HttpGet]
        [Route("SettingMainArticle")]
        public async Task<IActionResult> SettingMainArticle()
        {
            MainArticle AuthorArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, "mainArticle");
            var mainTags = await _tagService.GetAllTags();
            SettingMainArticleModel settingMainArticleModel = new SettingMainArticleModel { MainTags = mainTags, MainArticle = AuthorArticle };
            return View(settingMainArticleModel);   
        }
        [HttpPost]
        [Route("SettingMainArticle")]
        public async Task<IActionResult> SettingMainArticle(SettingMainArticleModel settingMainArticleModel)
        {
            MainArticle AuthorArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, "mainArticle");

            AuthorArticle.Title = settingMainArticleModel.MainArticle.Title;
            AuthorArticle.Description = settingMainArticleModel.MainArticle.Description;
            AuthorArticle.InfromationArticle = settingMainArticleModel.MainArticle.InfromationArticle;
            AuthorArticle.Price = settingMainArticleModel.MainArticle.Price;
            //AuthorArticle.supTags = settingMainArticleModel.MainArticle.supTags;

            SessionExtensionsMVC.Set(HttpContext.Session, "mainArticle", AuthorArticle);

            //var mainTags = await _tagService.GetAllTags();
            //SettingMainArticleModel settingMainArticleModel = new SettingMainArticleModel { MainTags = mainTags, MainArticle = AuthorArticle };
            return Redirect("~/All/AllBlogs");
        }
    }
}
