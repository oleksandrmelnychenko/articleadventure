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
        private readonly IUserService _userService;
        public UserController(ILogger<UserController> logger, IArticleService authenticationService, IUserService userService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _userService = userService;
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
            var userName = Request.Cookies[CookiesPath.USER_NAME];
            var surName = Request.Cookies[CookiesPath.SURNAME];
            var informationArticle = Request.Cookies[CookiesPath.INFORMATION_ARTICLE];

            UserProfile user = new UserProfile { UserName = userName, InformationAccount = informationArticle, SurName = surName };
            EditProfileModel editProfileModel = new EditProfileModel { UserProfile = user };
            return View(editProfileModel);
        }
        [HttpPost]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile(EditProfileModel editProfileModel)
        {
            var guidUser = User.FindFirst("Guid");

            editProfileModel.UserProfile.NetUid = Guid.Parse(guidUser.Value);
            await _userService.ChangeAccountInformation(editProfileModel.UserProfile);
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
        [HttpPost]
        [Route("AccountSecurity")]
        public async Task<IActionResult> AccountSecurity(AccountSecurityModel accountSecurityModel)
        {
            if (accountSecurityModel.NewPassword != accountSecurityModel.ConfirmNewPassword)
            {
                return View(accountSecurityModel);
            }
            var userGuidClaim = User.FindFirst("Guid");
            await _userService.ChangePassword(Guid.Parse(userGuidClaim.Value), accountSecurityModel.NewPassword, accountSecurityModel.CurrentPassword);
            return View(accountSecurityModel);
        }

        [HttpPost]
        [Route("AccountSecurityEmail")]
        public async Task<IActionResult> AccountSecurityEmail(AccountSecurityModel accountSecurityModel)
        {
            var userGuidClaim = User.FindFirst("Guid");

            await _userService.ChangeEmail(Guid.Parse(userGuidClaim.Value), accountSecurityModel.NewEmail, accountSecurityModel.ConfirmPasswordUpdateEmail);
            return View("AccountSecurity");
        }

    }
}