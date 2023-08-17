using domain.ArticleAdventure.Entities;
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
        private readonly IMainArticleService _mainArticleService;
        private readonly ITagService _tagService;


        public NewController(ILogger<NewController> logger, IArticleService authenticationService, IMainArticleService mainArticleService, ITagService tagService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _tagService = tagService;
            _mainArticleService = mainArticleService;
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

            var selectSupTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, "supTags");
            if (selectSupTags == null || selectSupTags.Count() == 0)
            {
                selectSupTags = new List<SupTag>();
            }
            selectSupTags.Add(findSupTag);
            SessionExtensionsMVC.Set(HttpContext.Session, "supTags", selectSupTags);
            return Redirect("~/SettingMainArticle");
        }

        [HttpGet]
        [Route("RemoveLocalSupTag")]
        public async Task<IActionResult> RemoveLocalSupTag(Guid RemoveSupTagsNetUid)
        {
            var selectSupTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, "supTags");

            var findSupTag = selectSupTags
            .Select(mainTag => mainTag)
            .FirstOrDefault(subTag => subTag.NetUid == RemoveSupTagsNetUid);
            selectSupTags.Remove(findSupTag);
            SessionExtensionsMVC.Set(HttpContext.Session, "supTags", selectSupTags);

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
            return Redirect("~/Login");
        }

        [HttpPost]
        [Route("SubArticle")]
        public async Task<IActionResult> SubArticle(NewArticleModel newBlogModel)
        {
            AuthorArticle createSubArticle = new AuthorArticle
            {
                Title = newBlogModel.authorArticle.Title,
                Body = newBlogModel.authorArticle.Body,
                Description = newBlogModel.authorArticle.Description,
                Price = newBlogModel.authorArticle.Price
            };

            NewArticleModel newArticleModel = new NewArticleModel { Title = newBlogModel.Title };

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
            var supTagsSelect = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, "supTags");

            if (supTagsSelect != null && supTagsSelect.Count() != 0)
            {
                //foreach (var mainTag in mainTags)
                //{
                //    foreach (var supTag in mainTag.SubTags)
                //    {
                //        var findSupTag = mainTags
                //        .SelectMany(mainTag => mainTag.SubTags)
                //        .FirstOrDefault(subTag => subTag.NetUid == supTag.NetUid);
                //    }
                //}
                foreach (var supTag in supTagsSelect)
                {
                    mainTags
                    .SelectMany(mainTag => mainTag.SubTags)
                    .First(subTag => subTag.NetUid == supTag.NetUid).IsSelected = true;
                }
            }

            SettingMainArticleModel settingMainArticleModel = new SettingMainArticleModel { MainTags = mainTags, MainArticle = AuthorArticle };
            return View(settingMainArticleModel);
        }
        [HttpPost]
        [Route("SettingMainArticle")]
        public async Task<IActionResult> SettingMainArticle(SettingMainArticleModel settingMainArticleModel)
        {
            MainArticle AuthorArticle = SessionExtensionsMVC.Get<MainArticle>(HttpContext.Session, "mainArticle");
            var selectSupTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, "supTags");

            AuthorArticle.Title = settingMainArticleModel.MainArticle.Title;
            AuthorArticle.Description = settingMainArticleModel.MainArticle.Description;
            AuthorArticle.InfromationArticle = settingMainArticleModel.MainArticle.InfromationArticle;
            AuthorArticle.Price = settingMainArticleModel.MainArticle.Price;
            AuthorArticle.ArticleTags = new List<MainArticleTags>();
            //AuthorArticle.supTags = settingMainArticleModel.MainArticle.supTags;
            //AuthorArticle.ArticleTags
            SessionExtensionsMVC.Set(HttpContext.Session, "mainArticle", AuthorArticle);

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
             await _mainArticleService.AddArticle(AuthorArticle);
            //var mainTags = await _tagService.GetAllTags();
            //SettingMainArticleModel settingMainArticleModel = new SettingMainArticleModel { MainTags = mainTags, MainArticle = AuthorArticle };
            return Redirect("~/All/AllBlogs");
        }
    }
}
