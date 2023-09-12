using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class InfoArticleModel
    {
        public MainArticle MainArticle { get; set; }
        public bool IsFavoriteArticle { get; set; }
        public Guid netUidFavoriteArticle { get; set; }
    }
}
