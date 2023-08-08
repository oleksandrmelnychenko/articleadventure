using domain.ArticleAdventure.Repositories.Blog;
using domain.ArticleAdventure.Repositories.Identity;
using domain.ArticleAdventure.Repositories.Identity.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories
{
    public sealed class IdentityRepositoriesFactory : IIdentityRepositoriesFactory
    {
        private readonly IIdentityRepository _identityRepository;

        public IdentityRepositoriesFactory(
            IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public IIdentityRepository NewIdentityRepository() => _identityRepository;

        public IUserTokenRepository NewUserTokenRepository(IDbConnection connection) =>
            new UserTokenRepository(connection);

        public IUserProfileRepository NewUserProfileRepository(IDbConnection connection) =>
            new UserProfileRepository(connection);
    }
}
