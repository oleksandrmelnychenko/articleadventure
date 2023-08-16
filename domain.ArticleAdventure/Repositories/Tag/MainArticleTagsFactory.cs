using domain.ArticleAdventure.Repositories.Tag.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Tag
{
    public class MainArticleTagsFactory : IMainArticleTagsFactory
    {
        public IMainArticleTagsService New(IDbConnection connection)
            => new MainArticleTagsService(connection);
    }
}
