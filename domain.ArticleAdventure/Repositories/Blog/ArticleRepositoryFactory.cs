using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Blog
{
    public class ArticleRepositoryFactory : IArticleRepositoryFactory
    {
        public IBlogRepository New(IDbConnection connection )
            => new ArticleRepository(connection);
    }
}
