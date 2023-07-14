using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Blog.Contracts
{
    public interface IBlogRepositoryFactory
    {
        IBlogRepository New(IDbConnection connection);
    }
}
