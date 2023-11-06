using Azure;
using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.EntityHelpers.Filter;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using System.Linq;

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

            ExecutionResult<List<MainTag>> mainTags = await _tagService.GetAllTags();
            List<MainArticleTags> mainArticleTags = new List<MainArticleTags>();
            List<long> supTagsId = new List<long>();
            MainArticleFilter mainArticleFilter1 = new MainArticleFilter();
            AllArticlesModel model = new AllArticlesModel { ArticleTags = mainTags.Data };

            var sessionStorageMainTags = SessionExtensionsMVC.Get<List<SupTag>>(HttpContext.Session, SessionStoragePath.CHOOSE_FILTER_SUP_TAGS);

            var mainArtilcesDateTime = await _mainArticleService.GetAllFilterDateTimeArticles();
            model.mainArticlesDateTime = mainArtilcesDateTime.Data;

            if (sessionStorageMainTags != null && sessionStorageMainTags.Count() != 0)
            {
                foreach (var supTag in sessionStorageMainTags)
                {
                    mainTags.Data
                    .SelectMany(mainTag => mainTag.SubTags)
                    .First(subTag => subTag.NetUid == supTag.NetUid).IsSelected = true;
                }
                supTagsId.AddRange(sessionStorageMainTags.Select(supTag => supTag.Id));
                mainArticleFilter1.SupTagsId = supTagsId;
                var mainArtilcesChanged = await _mainArticleService.GetAll(mainArticleFilter1);
                model.mainArticles = mainArtilcesChanged.Data;

            }
            else
            {
                List<MainArticle> mainArtilces = await _mainArticleService.GetAllArticles();
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