using domain.ArticleAdventure.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Blog.Contracts
{
    public interface IUserTokenRepository
    {
        void Add(UserToken userToken);

        void Update(UserToken userToken);

        void DeleteUserTokenByUserId(string userId);

        UserToken GetByUserIdIfExists(string userId);
    }
}
