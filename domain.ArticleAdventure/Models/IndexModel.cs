using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class IndexModel
    {
       public List<MainArticle> MainArticles {get;set;}
       public string JsonMainArticles {get;set; }
    }
}
