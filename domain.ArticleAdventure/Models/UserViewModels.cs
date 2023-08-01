using common.ArticleAdventure.ResponceBuilder.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class UserRegister
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [EmailAddress(ErrorMessage = "Field {0} with wring format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(100, ErrorMessage = "Field {0} need to have {2} e {1} character", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserLogin
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [EmailAddress(ErrorMessage = "Field {0} ewith wring format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(100, ErrorMessage = "Field {0} need to have {2} e {1} character", MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class UserResponseLogin
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserToken UserToken { get; set; }
        public IWebResponse ResponseResult { get; set; }
    }

    

    public class UserToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaim> Claims { get; set; }
    }

    public class UserClaim
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
