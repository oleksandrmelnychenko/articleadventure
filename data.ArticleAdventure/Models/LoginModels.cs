using Microsoft.AspNetCore.Mvc;

namespace data.ArticleAdventure.Models
{
    public class LoginModels
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Msg { get; set; }
    }
}
