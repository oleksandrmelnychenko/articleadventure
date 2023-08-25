using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    public class FavoriteArticle:EntityBase
    {
        public string UserId { get; set; }
        public UserProfile User  { get; set; }
        public string MainArticleId { get; set; }
        public MainArticle MainArticle { get; set; }
    }
}
