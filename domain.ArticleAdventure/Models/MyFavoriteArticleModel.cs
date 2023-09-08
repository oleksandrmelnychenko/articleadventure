using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class MyFavoriteArticleModel
    {
       public List<FavoriteArticle> favoriteArticles { get; set; }
    }
}
