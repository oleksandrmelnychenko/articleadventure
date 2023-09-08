using Azure;
using common.ArticleAdventure.ResponceBuilder;
using common.ArticleAdventure.WebApi;
using common.ArticleAdventure.WebApi.RoutingConfiguration;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json;

namespace MVC.ArticleAdventure.Controllers
{
    public class UserController : MVCControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IArticleService _articleService;
        private readonly IAuthService _authenticationService;
        private readonly IMainArticleService _mainArticleService;
        private readonly IUserService _userService;
        public UserController(ILogger<UserController> logger, IArticleService articleService, IUserService userService, IAuthService authService, IMainArticleService mainArticleService)
        {
            _logger = logger;
            _articleService = articleService;
            _userService = userService;
            _authenticationService = authService;
            _mainArticleService = mainArticleService;
        }

        [HttpGet]
        [Route("MyArticles")]
        public async Task<IActionResult> MyArticles()
        {
            var StringuserID = Request.Cookies[CookiesPath.USER_ID];
            var paymentArticles = await _mainArticleService.GetAllArticlesUser(long.Parse(StringuserID));
            MyArticlesModel myArticlesModel = new MyArticlesModel { mainArticles = paymentArticles.Data };
            return View(myArticlesModel);
        }

        [HttpGet]
        [Route("MyLists")]
        public async Task<IActionResult> MyLists()
        {
            return View();
        }
        [HttpGet]
        [Route("Profile")]
        public async Task<IActionResult> Profile(Guid netUidProfile)
        {
            if (netUidProfile != Guid.Empty)
            {
                var user = await _authenticationService.GetProfile(netUidProfile);
                ProfileModel model = new ProfileModel { InformationAccount = user.Data.InformationAccount, UserName = user.Data.UserName, SurName = user.Data.SurName };
                return View(model);

            }



            var userName = Request.Cookies[CookiesPath.USER_NAME];
            var surName = Request.Cookies[CookiesPath.SURNAME];
            var informationProfile = Request.Cookies[CookiesPath.INFORMATION_PROFILE];
            ProfileModel profileModel = new ProfileModel { InformationAccount = informationProfile, UserName = userName, SurName = surName };
            return View(profileModel);
        }
        [HttpGet]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            var userName = Request.Cookies[CookiesPath.USER_NAME];
            var surName = Request.Cookies[CookiesPath.SURNAME];
            var informationProfile = Request.Cookies[CookiesPath.INFORMATION_PROFILE];

            UserProfile user = new UserProfile { UserName = userName, InformationAccount = informationProfile, SurName = surName };
            EditProfileModel editProfileModel = new EditProfileModel { UserProfile = user };
            return View(editProfileModel);
        }
        [HttpPost]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile(EditProfileModel editProfileModel)
        {
            var guidUser = User.FindFirst("Guid");

            editProfileModel.UserProfile.NetUid = Guid.Parse(guidUser.Value);
            var result = await _userService.ChangeAccountInformation(editProfileModel.UserProfile);
            if (result.IsSuccess)
            {
                await SetSuccessMessage(SuccessMessages.UpdateProfile);
            }
            return View(editProfileModel);
        }

        [HttpGet]
        [Route("MyFavoriteArticle")]
        public async Task<IActionResult> MyFavoriteArticle()
        {
            MyFavoriteArticleModel myFavoriteArticleModel = new MyFavoriteArticleModel();
            var guidUser = User.FindFirst("Guid");
            var result = await _userService.GetAllFavoriteArticle(Guid.Parse(guidUser.Value));
            if (result.IsSuccess)
            {
                myFavoriteArticleModel.favoriteArticles = result.Data;
            }
            return View(myFavoriteArticleModel);
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
        [HttpGet]
        [Route("SetFavoriteArticle")]
        public async Task<IActionResult> SetFavoriteArticle(Guid netUidArticle)
        {
            var userGuidClaim = User.FindFirst("Guid");
            await _userService.SetFavoriteArticle(Guid.Parse(userGuidClaim.Value), netUidArticle);

            return Redirect($"~/InfoArticle{netUidArticle}");
        }
    }
}