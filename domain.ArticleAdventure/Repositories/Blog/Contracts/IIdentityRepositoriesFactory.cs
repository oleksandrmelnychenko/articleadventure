using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Blog.Contracts
{
    public interface IIdentityRepositoriesFactory
    {
        IIdentityRepository NewIdentityRepository();

        IUserTokenRepository NewUserTokenRepository(IDbConnection connection);

        IUserProfileRepository NewUserProfileRepository(IDbConnection connection);
    }
}
