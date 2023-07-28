using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class ChangeBlogModel
    {
        public Guid NetUid { get; set; }
        public AuthorArticle Article { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
    }
}
