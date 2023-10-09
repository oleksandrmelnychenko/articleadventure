using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class RegisterModel
    {
        public string Role { get; set; } = "";
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(60, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(60, MinimumLength = 3)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "password does not match")]
        public string Password { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;

        public Guid NetUidPriofile { get; set; }
    }
}
