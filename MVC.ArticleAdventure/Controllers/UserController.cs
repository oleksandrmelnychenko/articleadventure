using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> MyAllArticles()
        {
            return View();
        }
    }
}