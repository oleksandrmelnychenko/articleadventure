using Azure;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;

namespace MVC.ArticleAdventure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _supArticleService;
        private readonly IMainArticleService _mainArticleService;
        private readonly ITagService _tagService;
        private readonly IStripeService _stripeService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IArticleService authenticationService,
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
        public async Task<IActionResult> Index()
        {

            var foo = User.GetUserToken();

            var _foo = User.Claims;

            List<MainTag> mainTags = await _tagService.GetAllTags();
            List<MainArticleTags> mainArticleTags = new List<MainArticleTags>();
            AllArticlesModel model = new AllArticlesModel { ArticleTags = mainTags };

            var sessionStorageMainTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHOOSE_FILTER_SUP_TAGS);

            if (sessionStorageMainTags != null && sessionStorageMainTags.Count() != 0)
            {

                foreach (var supTag in sessionStorageMainTags)
                {
                    mainTags
                    .SelectMany(mainTag => mainTag.SubTags)
                    .First(subTag => subTag.NetUid == supTag.NetUid).IsSelected = true;
                }
            }
            var mainArtilces = await _mainArticleService.GetAllArticles();
            var mainArtilcesDateTime = await _mainArticleService.GetAllFilterDateTimeArticles();
            model.mainArticlesDateTime = mainArtilcesDateTime.Data;
            if (sessionStorageMainTags != null && sessionStorageMainTags.Count() != 0)
            {
                foreach (var mainArticle in mainArtilces)
                {
                    foreach (var tag in mainArticle.ArticleTags)
                    {
                        if (sessionStorageMainTags.Any(x => x.Id == tag.SupTagId))
                        {
                            mainArticleTags.Add(tag);
                        }

                    }
                }
                var mainArticleFilter = await _mainArticleService.GetAllArticlesFilterSupTags(mainArticleTags);
                model.mainArticles = mainArticleFilter.Data;
            }
            else
            {
                model.mainArticles = mainArtilces;
            }
            return View(model);
        }
       

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

    }
}