using Azure;
using domain.ArticleAdventure.Helpers;
using Microsoft.AspNetCore.Mvc;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;

namespace MVC.ArticleAdventure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthService _authenticationService;

        public HomeController(ILogger<HomeController> logger, IAuthService authService)
        {
            _authenticationService = authService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

    }
}