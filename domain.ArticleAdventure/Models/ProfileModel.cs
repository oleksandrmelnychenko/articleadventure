using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class ProfileModel
    {
        public UserProfile Profile { get; set; } 
        public List<MainArticle> FavoriteMainArticle { get; set; }
        public List<StripePayment> historyArticleBuy { get; set; }
        public string UserName { get; set; }
        public string SurName { get; set; }
        public string InformationAccount { get; set; }

        public ProfileModel()
        {
            Profile = new UserProfile { mainArticles = new List<MainArticle>() };
        }
    }
}
 