using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Services.Contract;

namespace MVC.ArticleAdventure.Controllers
{
    public class IdentityController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthService _authenticationService;

        public IdentityController(ILogger<HomeController> logger, IAuthService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
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
            if (!ModelState.IsValid) return View(userLogin);


            var response = await _authenticationService.Login(userLogin);
            return View();
        }
        [HttpGet]
        [Route("CreateAccount")]
        public async Task<IActionResult> CreateAccount()
        {
            return View();
        }
        [HttpGet]
        [Route("SendMail")]
        public async Task<IActionResult> SendMail()
        {
            return View();
        }
    }
}
