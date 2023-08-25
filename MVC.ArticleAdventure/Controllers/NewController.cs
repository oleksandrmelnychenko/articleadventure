﻿using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using domain.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Helpers;

namespace MVC.ArticleAdventure.Controllers
{
    public class NewController : Controller
    {
        private readonly ILogger<NewController> _logger;
        private readonly IArticleService _supArticleService;
        private readonly IMainArticleService _mainArticleService;
        private readonly ITagService _tagService;


        public NewController(ILogger<NewController> logger, IArticleService authenticationService, IMainArticleService mainArticleService, ITagService tagService)
        {
            _logger = logger;
            _supArticleService = authenticationService;
            _tagService = tagService;
            _mainArticleService = mainArticleService;
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
        [Route("IsSelectLocalSupTag")]
        public async Task<IActionResult> IsSelectLocalSupTag(Guid IsSelectSupTagsNetUid)
        {
            var mainTags = await _tagService.GetAllTags();
            var findSupTag = mainTags
            .SelectMany(mainTag => mainTag.SubTags)
            .FirstOrDefault(subTag => subTag.NetUid == IsSelectSupTagsNetUid);

            var selectSupTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHOOSE_NEW_SUP_TAGS);
            if (selectSupTags == null || selectSupTags.Count() == 0)
            {
                selectSupTags = new List<SupTag>();
            }
            selectSupTags.Add(findSupTag);
            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHOOSE_NEW_SUP_TAGS, selectSupTags);
            return Redirect("~/SettingMainArticle");
        }

     


        [HttpGet]
        [Route("RemoveLocalSupTag")]
        public async Task<IActionResult> RemoveLocalSupTag(Guid RemoveSupTagsNetUid)
        {
            var selectSupTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHOOSE_NEW_SUP_TAGS);

            var findSupTag = selectSupTags
            .Select(mainTag => mainTag)
            .FirstOrDefault(subTag => subTag.NetUid == RemoveSupTagsNetUid);
            selectSupTags.Remove(findSupTag);
            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHOOSE_NEW_SUP_TAGS, selectSupTags);

            return Redirect("~/SettingMainArticle");
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
            var userGuidClaim = User.FindFirst("Guid");
            return Redirect("~/Login");
        }

       

        [HttpGet]
        [Route("AddSupArticle")]
        public async Task<IActionResult> AddSupArticle(Guid netUidMainArticle)
        {
            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.ID_MAIN_ARTICLE, netUidMainArticle);

            return View();
        }

        [HttpPost]
        [Route("AddSupArticle")]
        public async Task<IActionResult> AddSupArticle(NewArticleModel NewArticleModel)
        {
            Guid authorArticleNetUid = SessionExtensionsMVC.Get<Guid>(HttpContext.Session, SessionStoragePath.ID_MAIN_ARTICLE);
            MainArticle mainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE);

            NewArticleModel.authorArticle.MainArticleId = mainArticle.Id;
            await _supArticleService.AddArticle(NewArticleModel.authorArticle);
            mainArticle.Articles.Add(NewArticleModel.authorArticle);

            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE, mainArticle);


            return Redirect($"~/ChangeMainArticle?netUidArticle={authorArticleNetUid}");
        }

        [HttpGet]
        [Route("RemoveLocalSupArticle")]
        [Authorize]
        public async Task<IActionResult> RemoveLocalSupArticle(long RemoveIdArticle)
        {

            var mainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CREATE_MAIN_ARTICLE);

            var supArticle = mainArticle.Articles.First(x => x.Id == RemoveIdArticle);

            mainArticle.Articles.Remove(supArticle);


            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CREATE_MAIN_ARTICLE, mainArticle);

            return Redirect("~/SettingMainArticle");
        }
        [HttpGet]
        [Route("ChangeLocalSupArticle")]
        [Authorize]
        public async Task<IActionResult> ChangeLocalSupArticle(long ChangeIdArticle)
        {

            var mainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CREATE_MAIN_ARTICLE);

            var supArticle = mainArticle.Articles.First(x => x.Id == ChangeIdArticle);

            ChangeArticleModel changeArticleModel = new ChangeArticleModel { Article = supArticle };
            return View(changeArticleModel);
        }

        [HttpPost]
        [Route("ChangeLocalSupArticle")]
        [Authorize]
        public async Task<IActionResult> ChangeSupArticle(ChangeArticleModel changeArticleModel)
        {
            var mainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CREATE_MAIN_ARTICLE);

            var supArticle = mainArticle.Articles.FirstOrDefault(x => x.Id == changeArticleModel.Article.Id);

            supArticle.Title = changeArticleModel.Article.Title;
            supArticle.Description = changeArticleModel.Article.Description;
            supArticle.Price = changeArticleModel.Article.Price;
            supArticle.Body = changeArticleModel.Article.Body;

            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CREATE_MAIN_ARTICLE, mainArticle);


            return Redirect("~/SettingMainArticle");
        }

        [HttpGet]
        [Route("AddNewSupArticle")]
        public async Task<IActionResult> AddNewSupArticle()
        {
            return View();
        }
        [HttpPost]
        [Route("AddNewSupArticle")]
        public async Task<IActionResult> AddNewSupArticle(NewArticleModel NewArticleModel)
        {
            MainArticle AuthorArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CREATE_MAIN_ARTICLE);
            
            NewArticleModel.authorArticle.Id = AuthorArticle.Articles.Count();
            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CREATE_SUP_ARTICLE, NewArticleModel.authorArticle);

            return Redirect("~/SettingMainArticle");
        }

        [HttpGet]
        [Route("SettingMainArticle")]
        public async Task<IActionResult> SettingMainArticle()
        {
            MainArticle mainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CREATE_MAIN_ARTICLE);
            if (mainArticle == null)
            {
                mainArticle = new MainArticle();
            }

            var mainTags = await _tagService.GetAllTags();
            var supTagsSelect = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHOOSE_NEW_SUP_TAGS);
            var supArticle = SessionExtensionsMVC.Get<AuthorArticle>(HttpContext.Session, SessionStoragePath.CREATE_SUP_ARTICLE);

            if (supArticle != null )
            {
                mainArticle.Articles.Add(supArticle);
                HttpContext.Session.Remove(SessionStoragePath.CREATE_SUP_ARTICLE);
            }

            if (supTagsSelect != null && supTagsSelect.Count() != 0)
            {
                foreach (var supTag in supTagsSelect)
                {
                    mainTags
                    .SelectMany(mainTag => mainTag.SubTags)
                    .First(subTag => subTag.NetUid == supTag.NetUid).IsSelected = true;
                }
            }
            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CREATE_MAIN_ARTICLE, mainArticle);

            SettingMainArticleModel settingMainArticleModel = new SettingMainArticleModel { MainTags = mainTags, MainArticle = mainArticle };
            return View(settingMainArticleModel);
        }


        [HttpPost]
        [Route("SettingMainArticle")]
        public async Task<IActionResult> SettingMainArticle(SettingMainArticleModel settingMainArticleModel)
        {
            MainArticle AuthorArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CREATE_MAIN_ARTICLE);
            var selectSupTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHOOSE_NEW_SUP_TAGS);

            AuthorArticle.Title = settingMainArticleModel.MainArticle.Title;
            AuthorArticle.Description = settingMainArticleModel.MainArticle.Description;
            AuthorArticle.InfromationArticle = settingMainArticleModel.MainArticle.InfromationArticle;
            AuthorArticle.Price = settingMainArticleModel.MainArticle.Price;
            AuthorArticle.ArticleTags = new List<MainArticleTags>();

            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CREATE_MAIN_ARTICLE, AuthorArticle);

            foreach (var item in selectSupTags)
            {
                var mainTag = new MainArticleTags
                {
                    MainArticleId = AuthorArticle.Id,
                    SupTag = item,
                    SupTagId = item.Id,
                };
                AuthorArticle.ArticleTags.Add(mainTag);
            }
            HttpContext.Session.Remove(SessionStoragePath.CREATE_MAIN_ARTICLE);
            HttpContext.Session.Remove(SessionStoragePath.CHOOSE_NEW_SUP_TAGS);
            await _mainArticleService.AddArticle(AuthorArticle);
            return Redirect("~/All/AllBlogs");
        }
    }
}
