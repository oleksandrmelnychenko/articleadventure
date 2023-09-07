using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class GetInformationArticleModel
    {
        public MainArticle MainArticle { get; set; }
        public List<AuthorArticle> NotBuyArticle { get; set; }
        public GetInformationArticleModel()
        {
            NotBuyArticle = new List<AuthorArticle>();
        }

    }
}
