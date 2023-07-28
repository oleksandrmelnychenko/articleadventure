using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class AllArticlesModel
    {
        public List<AuthorArticle> Articles { get; set; }
        public Guid NetUid { get; set; }
    }
}
