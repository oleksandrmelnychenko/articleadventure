using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Tag.Contracts
{
    public interface IMainArticleTagsFactory
    {
        IMainArticleTagsService New(IDbConnection connection);
    }
}
