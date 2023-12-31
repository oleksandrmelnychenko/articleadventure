﻿using common.ArticleAdventure.IdentityConfiguration;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.WebApi.RoutingConfiguration;
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
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services;
using MVC.ArticleAdventure.Services.Contract;
using System.Data;
using System.Security.Claims;

namespace MVC.ArticleAdventure.Controllers
{
    public class AllController : MVCControllerBase
    {
        private readonly ILogger<AllController> _logger;
        private readonly IArticleService _supArticleService;
        private readonly IMainArticleService _mainArticleService;
        private readonly ITagService _tagService;
        private readonly IStripeService _stripeService;
        private readonly IUserService _userService;

        public AllController(ILogger<AllController> logger, IArticleService authenticationService,
            IMainArticleService mainArticleService, ITagService tagService,
            IStripeService stripeService, IUserService userService)
        {
            _logger = logger;
            _supArticleService = authenticationService;
            _mainArticleService = mainArticleService;
            _tagService = tagService;
            _stripeService = stripeService;
            _userService = userService;
        }

        [HttpGet]
        [Route("ChangeMainArticle")]
        [Authorize]
        public async Task<IActionResult> ChangeMainArticle(Guid netUidArticle)
        {
            var mainArticle = await _mainArticleService.GetArticle(netUidArticle);
            var mainTags = await _tagService.GetAllTags();
            ChangeMainArticleModel changeArticleModel = new ChangeMainArticleModel { MainArticle = mainArticle, MainTags = mainTags.Data };

            var sessionStorageMainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE);
            var sessionStorageMainTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_TAGS);

            if (sessionStorageMainTags != null && sessionStorageMainTags.Count() != 0)
            {

                foreach (var supTag in sessionStorageMainTags)
                {
                    mainTags.Data
                    .SelectMany(mainTag => mainTag.SubTags)
                    .First(subTag => subTag.NetUid == supTag.NetUid).IsSelected = true;
                }
            }
            if (sessionStorageMainTags == null)
            {
                var selectSupTags = new List<SupTag>();
                foreach (var item in mainTags.Data)
                {
                    selectSupTags = mainArticle.ArticleTags.Select(x => x.SupTag).ToList();

                }
                foreach (var supTag in selectSupTags)
                {
                    mainTags.Data
                    .SelectMany(mainTag => mainTag.SubTags)
                    .First(subTag => subTag.NetUid == supTag.NetUid).IsSelected = true;
                }
                changeArticleModel.MainTags = mainTags.Data;
                SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_TAGS, selectSupTags);
            }


            if (sessionStorageMainArticle == null)
            {
                SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE, mainArticle);
                return View(changeArticleModel);
            }
            else
            {
                if (mainArticle.NetUid != sessionStorageMainArticle.NetUid)
                {
                    SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE, mainArticle);
                }
                else
                {
                    changeArticleModel.MainArticle = sessionStorageMainArticle;
                }
                return View(changeArticleModel);
            }
        }

        [HttpPost]
        [Route("ChangeMainArticle")]
        [Authorize]
        public async Task<IActionResult> ChangeMainArticle(ChangeMainArticleModel changeArticleModel)
        {
            var mainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE);
            var selectSupTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_TAGS);
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            mainArticle.Title = changeArticleModel.MainArticle.Title;
            mainArticle.InfromationArticle = changeArticleModel.MainArticle.InfromationArticle;
            mainArticle.Description = changeArticleModel.MainArticle.Description;
            mainArticle.Price = changeArticleModel.MainArticle.Price;

            mainArticle.ArticleTags.Clear();
            if (selectSupTags != null)
            { 
                foreach (var item in selectSupTags)
                {
                    var mainTag = new MainArticleTags
                    {
                        MainArticleId = mainArticle.Id,
                        SupTag = item,
                        SupTagId = item.Id,
                    };
                    mainArticle.ArticleTags.Add(mainTag);
                }
            }

            var response = await _mainArticleService.Update(mainArticle, changeArticleModel.PhotoMainArticle, token);
            if (response.IsSuccess)
            {
                HttpContext.Session.Remove(SessionStoragePath.CHANGE_MAIN_ARTICLE);
                HttpContext.Session.Remove(SessionStoragePath.CHANGE_MAIN_TAGS);
                await SetSuccessMessage(SuccessMessages.UpdateArticle);
                return Redirect("~/");
            }
            else
            {
                await SetErrorMessage(response.Error.Message);
                return View(changeArticleModel);
            }

        }

        [HttpGet]
        [Route("IsSelectChangeLocalSupTag")]
        public async Task<IActionResult> IsSelectChangeLocalSupTag(Guid IsSelectSupTagsNetUid)
        {
            var mainTags = await _tagService.GetAllTags();
            var findSupTag = mainTags.Data
            .SelectMany(mainTag => mainTag.SubTags)
            .FirstOrDefault(subTag => subTag.NetUid == IsSelectSupTagsNetUid);

            var selectSupTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_TAGS);
            var sessionStorageMainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE);

            if (selectSupTags == null || selectSupTags.Count() == 0)
            {
                selectSupTags = new List<SupTag>();
            }
            selectSupTags.Add(findSupTag);
            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_TAGS, selectSupTags);
            return Redirect($"~/ChangeMainArticle?netUidArticle={sessionStorageMainArticle.NetUid}");
        }

        [HttpGet]
        [Route("CancelChangeLocalSupTag")]
        public async Task<IActionResult> CancelChangeLocalSupTag(Guid RemoveSupTagsNetUid)
        {
            var selectSupTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_TAGS);
            var sessionStorageMainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE);


            var findSupTag = selectSupTags
            .Select(mainTag => mainTag)
            .FirstOrDefault(subTag => subTag.NetUid == RemoveSupTagsNetUid);
            selectSupTags.Remove(findSupTag);
            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_TAGS, selectSupTags);

            return Redirect($"~/ChangeMainArticle?netUidArticle={sessionStorageMainArticle.NetUid}");
        }


        [HttpGet]
        [Route("GetInformationArticle")]
        [Authorize]
        public async Task<IActionResult> GetInformationArticle(Guid netUidArticle)
        {
            var userId = long.Parse(Request.Cookies[CookiesPath.USER_ID]);
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            GetInformationArticleModel changeArticleModel = new GetInformationArticleModel();
            var responseMainArticle = await _mainArticleService.GetArticleUser(netUidArticle, userId,token);
            var mainArtilce = await _mainArticleService.GetArticle(netUidArticle);

            foreach (var SupArticle in mainArtilce.Articles)
            {
                if (!responseMainArticle.Data.Articles.Any(x => x.Id.Equals(SupArticle.Id)))
                {
                    changeArticleModel.NotBuyArticle.Add(SupArticle);
                }
            }
            if (responseMainArticle.IsSuccess)
            {
                if (responseMainArticle.Data != null)
                {
                    changeArticleModel.MainArticle = responseMainArticle.Data;
                    return View(changeArticleModel);
                }
            }
            return Redirect($"~/InfoArticle?netUidArticle={netUidArticle}");
        }



        [HttpGet]
        [Route("ChangeSupArticle")]
        [Authorize]
        public async Task<IActionResult> ChangeSupArticle(Guid ChangeNetUidArticle)
        {
            var mainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE);

            var supArticle = mainArticle.Articles.First(x => x.NetUid == ChangeNetUidArticle);

            var article = await _mainArticleService.GetArticle(ChangeNetUidArticle);
            ChangeArticleModel changeArticleModel = new ChangeArticleModel { Article = supArticle };
            return View(changeArticleModel);
        }

        [HttpGet]
        [Route("RemoveSupArticle")]
        [Authorize]
        public async Task<IActionResult> RemoveSupArticle(Guid RemoveNetUidArticle)
        {
            var mainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE);
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            var supArticle = mainArticle.Articles.First(x => x.NetUid == RemoveNetUidArticle);

            mainArticle.Articles.Remove(supArticle);

            await _supArticleService.Remove(RemoveNetUidArticle, token);

            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE, mainArticle);

            ChangeArticleModel changeArticleModel = new ChangeArticleModel { Article = supArticle };
            return Redirect($"~/ChangeMainArticle?netUidArticle={mainArticle.NetUid}");
        }

        [HttpPost]
        [Route("ChangeSupArticle")]
        [Authorize/*(Roles = IdentityRoles.Administrator)*/]
        public async Task<IActionResult> ChangeSupArticle(ChangeArticleModel changeBlogModel)
        {
            var mainArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE);
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            var article = mainArticle.Articles.FirstOrDefault(article => article.NetUid == changeBlogModel.Article.NetUid);
            article.Title = changeBlogModel.Article.Title;
            article.Description = changeBlogModel.Article.Description;
            article.Body = changeBlogModel.Article.Body;
            article.Price = changeBlogModel.Article.Price;
            await _supArticleService.Update(article,token);
            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHANGE_MAIN_ARTICLE, mainArticle);

            return Redirect($"~/ChangeMainArticle?netUidArticle={mainArticle.NetUid}");
        }


        [HttpGet]
        [Route("IsSelectFilterSupTag")]
        public async Task<IActionResult> IsSelectFilterSupTag(Guid IsSelectSupTagsNetUid)
        {
            var mainTags = await _tagService.GetAllTags();
            var findSupTag = mainTags.Data
            .SelectMany(mainTag => mainTag.SubTags)
            .FirstOrDefault(subTag => subTag.NetUid == IsSelectSupTagsNetUid);

            var filterSupTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHOOSE_FILTER_SUP_TAGS);
            if (filterSupTags == null || filterSupTags.Count() == 0)
            {
                filterSupTags = new List<SupTag>();
            }
            filterSupTags.Add(findSupTag);
            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHOOSE_FILTER_SUP_TAGS, filterSupTags);
            return Redirect("~/");
        }

        [HttpGet]
        [Route("RemoveFilterlSupTag")]
        public async Task<IActionResult> RemoveFilterlSupTag(Guid RemoveSupTagsNetUid)
        {
            var filterSupTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHOOSE_FILTER_SUP_TAGS);

            var findSupTag = filterSupTags
            .Select(mainTag => mainTag)
            .FirstOrDefault(subTag => subTag.NetUid == RemoveSupTagsNetUid);
            filterSupTags.Remove(findSupTag);
            SessionExtensionsMVC.Set(HttpContext.Session, SessionStoragePath.CHOOSE_FILTER_SUP_TAGS, filterSupTags);

            return Redirect("~/");
        }

        [Route("RemoveMainArticle")]
        [Authorize]
        public async Task<IActionResult> RemoveMainArticle(Guid netUidArticle)
        {
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            await _mainArticleService.Remove(netUidArticle,token);
            return Redirect("~/");
        }

        [HttpGet]
        [Route("InfoArticle")]
        public async Task<IActionResult> InfoArticle(Guid NetUidArticle)
        {
            InfoArticleModel infoArticleModel = new InfoArticleModel();
            var article = await _mainArticleService.GetArticle(NetUidArticle);
            var listAutorArticle = SessionExtensionsMVC.Get<List<MainArticle>>(HttpContext.Session, SessionStoragePath.CART_ARTICLE);
            var token = Request.Cookies[CookiesPath.ACCESS_TOKEN];

            if (listAutorArticle != null && listAutorArticle.Count != 0)
            {
                infoArticleModel.IsSetBasket = listAutorArticle.Any(x => x.Id == article.Id);
            }

            var userGuidClaim = User.FindFirst("Guid");
            var email = Request.Cookies[CookiesPath.EMAIL];
            if (UserRoleHelper.IsUserRole(User.Claims,"User")|| UserRoleHelper.IsUserRole(User.Claims, Roles.Admin))
            {
                var favoriteArticle = await _userService.GetFavoriteArticle(Guid.Parse(userGuidClaim.Value), NetUidArticle,token);

                if (favoriteArticle.IsSuccess)
                {
                    if (favoriteArticle.Data != null)
                    {
                        infoArticleModel.netUidFavoriteArticle = favoriteArticle.Data.NetUid;
                        infoArticleModel.IsFavoriteArticle = true;
                    }
                }
                var payments = await _stripeService.CheckPaymentsHaveUser(email);

                var articles = ConvertPaymentsIntoArticles.Convert(payments.Data);
                var checkArticle = payments.Data.Where(x => x.MainArticleId == article.Id).ToList();

                if (checkArticle.Count() != 0)
                {
                    return Redirect($"~/GetInformationArticle/?NetUidArticle={article.NetUid}");
                }
            }

            
            infoArticleModel.MainArticle = article;
            return View(infoArticleModel);
        }
    }
}
