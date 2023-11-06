using domain.ArticleAdventure.EntityHelpers.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.FilterBuilders.MainArticles
{
    public interface IMainArticleFilterBuilder
    {
        string Build(MainArticleFilter filter);

    }
}
