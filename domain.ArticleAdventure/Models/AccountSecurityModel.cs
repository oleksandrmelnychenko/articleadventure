using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class AccountSecurityModel
    {
        public UserProfile ChangeUser { get; set; }

        public string NewEmail { get; set; }
        public string ConfirmPasswordUpdateEmail { get; set; }


        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

    }
}
