using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class ChangeMainArticleModel
    {
        public MainArticle MainArticle { get; set; }
        public List<MainTag> MainTags { get; set; }
    }
}
