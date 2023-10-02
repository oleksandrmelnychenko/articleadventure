using common.ArticleAdventure.IdentityConfiguration;
using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class UserManagerModel
    {
        public List<UserProfile> UserProfiles { get; set; }

        public UserProfile EditUserProfile { get; set; }

        public bool CreateUser { get; set; }
        public bool EditUser { get; set; }

        public int SelectedRow { get; set; } = -1;
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string selectedRole { get; set; }
        public List<string> IdentityRoles { get; set; } = new List<string>();
    }
}
