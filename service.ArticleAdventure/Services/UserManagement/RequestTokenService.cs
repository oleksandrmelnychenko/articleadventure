using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.EntityHelpers.Identity;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.IdentityEntities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using Newtonsoft.Json;
using service.ArticleAdventure.Services.UserManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services.UserManagement
{
    public sealed class RequestTokenService : IRequestTokenService
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IIdentityRepositoriesFactory _identityRepositoriesFactory;

        public RequestTokenService(
            IDbConnectionFactory connectionFactory,
            IIdentityRepositoriesFactory identityRepositoriesFactory)
        {
            _connectionFactory = connectionFactory;
            _identityRepositoriesFactory = identityRepositoriesFactory;
        }

        public Task<CompleteAccessToken> RequestToken(string userName, string password, bool rememberUser) =>
            Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(userName)) throw new Exception("Please enter your Email");

                if (string.IsNullOrEmpty(password)) throw new Exception("Please enter your Password");

                (ClaimsIdentity claims, User user) =
                    await _identityRepositoriesFactory
                        .NewIdentityRepository()
                        .AuthAndGetClaimsIdentity(
                            userName,
                            password
                        );

                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return TokenGenerator.GenerateToken(
                        _identityRepositoriesFactory.NewUserTokenRepository(connection),
                        claims.Claims,
                        user,
                        rememberUser
                    );
                }
            });

        public Task<CompleteAccessToken> RefreshToken(string refreshToken) =>
            Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(refreshToken)) throw new Exception("Refresh token is invalid");

                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IUserTokenRepository userTokenRepository = _identityRepositoriesFactory.NewUserTokenRepository(connection);

                    string decryptedToken = AesManager.Decrypt(refreshToken);

                    RefreshToken deserializedRefreshToken = JsonConvert.DeserializeObject<RefreshToken>(decryptedToken);

                    if (deserializedRefreshToken.ExpireAt < DateTime.UtcNow) throw new Exception("Refresh token expired");

                    UserToken userToken = userTokenRepository.GetByUserIdIfExists(deserializedRefreshToken.UserId);

                    if (userToken == null || userToken.Token != refreshToken.Replace(" ", "+")) throw new Exception("Refresh token is invalid");

                    RefreshToken deserializedExistingToken = JsonConvert.DeserializeObject<RefreshToken>(AesManager.Decrypt(userToken.Token));

                    Tuple<ClaimsIdentity, User> authResult =
                        await _identityRepositoriesFactory
                            .NewIdentityRepository()
                            .AuthAndGetClaimsIdentity(
                                deserializedRefreshToken.UserId
                            );

                    if (authResult == null) throw new Exception("Refresh token is invalid");

                    return TokenGenerator.GenerateToken(
                        userTokenRepository,
                        authResult.Item1.Claims,
                        authResult.Item2,
                        Convert.ToInt32(deserializedExistingToken.ExpireAt.Subtract(DateTime.UtcNow).TotalMinutes)
                    );
                }
            });

        public Task DeleteRefreshTokenOnLogoutByUserId(Guid userNetId) =>
            Task.Run(async () =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    _identityRepositoriesFactory
                        .NewUserTokenRepository(connection)
                        .DeleteUserTokenByUserId(
                            await _identityRepositoriesFactory.NewIdentityRepository().GetUserIdByUserNetId(userNetId)
                        );
                }
            });
    }
}
