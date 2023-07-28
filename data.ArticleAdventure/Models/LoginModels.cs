using Microsoft.AspNetCore.Mvc;

namespace data.ArticleAdventure.Models
{
    public class LoginModels
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

    }
}
