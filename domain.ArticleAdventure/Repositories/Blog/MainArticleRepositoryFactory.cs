using domain.ArticleAdventure.Repositories.Blog.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Blog
{
    public class MainArticleRepositoryFactory : IMainArticleRepositoryFactory
    {
        public IMainArticleRepository New(IDbConnection connection)
            => new MainArticleRepository(connection);
    }
}
