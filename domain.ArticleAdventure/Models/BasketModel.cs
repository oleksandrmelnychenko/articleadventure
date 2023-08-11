using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class BasketModel
    {
        public List<AuthorArticle> BasketArticles { get; set; }
    }
}
