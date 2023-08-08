using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class ChangeArticleModel
    {
        public Guid NetUid { get; set; }
        public AuthorArticle Article { get; set; }

    }
}
