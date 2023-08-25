using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;

namespace MVC.ArticleAdventure.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IArticleService _authenticationService;
        public UserController(ILogger<UserController> logger, IArticleService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [Route("MyArticles")]
        public async Task<IActionResult> MyArticles()
        {
            return View();
        }

        [HttpGet]
        [Route("MyLists")]
        public async Task<IActionResult> MyLists()
        {
            return View();
        }
        [HttpGet]
        [Route("MyProfile")]
        public async Task<IActionResult> MyProfile()
        {
            return View();
        }
        [HttpGet]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            var user = SessionExtensionsMVC.Get<UserProfile>(HttpContext.Session, SessionStoragePath.USER);
            EditProfileModel editProfileModel = new EditProfileModel { UserProfile = user };
            return View(editProfileModel);
        }

        [HttpGet]
        [Route("MyFavoriteArticle")]
        public async Task<IActionResult> MyFavoriteArticle()
        {
            return View();
        }

        [HttpGet]
        [Route("HistoryBuy")]
        public async Task<IActionResult> HistoryBuy()
        {
            return View();
        }
        [HttpGet]
        [Route("AccountSecurity")]
        public async Task<IActionResult> AccountSecurity()
        {
            var user = SessionExtensionsMVC.Get<UserProfile>(HttpContext.Session, SessionStoragePath.USER);
            AccountSecurityModel accountSecurityModel = new AccountSecurityModel { ChangeUser = user };
            return View(accountSecurityModel);
        }
        
    }
}