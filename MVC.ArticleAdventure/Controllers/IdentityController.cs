using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVC.ArticleAdventure.Controllers
{
    public class IdentityController : Controller
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly IAuthService _authenticationService;
        private readonly IUserProfileService _userProfileService;

        public IdentityController(ILogger<IdentityController> logger, IAuthService authenticationService, IUserProfileService userProfileService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _userProfileService = userProfileService;
        }

        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login()
        {
            //_authenticationService.Login();
            return View();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(userLogin);
            }
            UserResponseLogin response = await _authenticationService.Login(userLogin);
            if (response.ResponseResult != null)
            {
                //добавити сповіщення про неправильний логін
                return View();
            }
            else
            {
                await SignIn(response);
                return Redirect("/All/AllBlogs");
            }
            
        }

        private async Task SignIn(UserResponseLogin response)
        {
            var token = GetTokenFormat(response.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", response.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true //Multiple requests
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        private static JwtSecurityToken GetTokenFormat(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
        [HttpGet]
        [Route("CreateAccount")]
        public async Task<IActionResult> CreateAccount()
        {
            RegisterModel registerModel = new RegisterModel { IsEmailConfirmed = false};
            return View(registerModel);
        }
        [HttpPost]
        [Route("CreateAccount")]
        public async Task<IActionResult> CreateAccount(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }
            var token = await _userProfileService.CreateAccount(registerModel);
            if (token.ResponseResult!= null)
            {
                //тут треба щось на перевірте почту
                return View(registerModel);
            }
            else
            {
                registerModel.IsEmailConfirmed = true;
                return View(registerModel);
            }
        }


        [HttpGet]
        [Route("emailConfirmation")]
        public async Task<IActionResult> emailConfirmation(string access_token,string userId)
        {
           var resultSucceeded =  await _userProfileService.EmailConformation(access_token, userId);
            if (resultSucceeded)
            {
                return View();
            }
            else
            {
                return View();
            }
            
        }
    }
}
