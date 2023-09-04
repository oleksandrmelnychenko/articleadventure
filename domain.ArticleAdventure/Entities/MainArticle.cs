using common.ArticleAdventure.WebApi.RoutingConfiguration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    public class MainArticle:EntityBase
    {
        //[Required(ErrorMessage = "Field {0} is required")]
        public string Title { get; set; }
        //[Required(ErrorMessage = "Field {0} is required")]
        public string Description { get; set; }
        public string Image { get; set; }
        public string ImageUrl { get; set; }
        public string WebImageUrl { get; set; }
        public List<AuthorArticle> Articles { get; set; }
        //[Required(ErrorMessage = "Field {0} is required")]
        public string InfromationArticle { get; set; }
        public  List<MainArticleTags> ArticleTags { get; set; }
        public double Price { get; set; }

        public MainArticle()
        {
            Articles = new List<AuthorArticle>();
            ArticleTags = new List<MainArticleTags>();
        }
    }
}
