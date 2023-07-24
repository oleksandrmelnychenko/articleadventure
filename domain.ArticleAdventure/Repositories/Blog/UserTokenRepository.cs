using Dapper;
using domain.ArticleAdventure.IdentityEntities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Blog
{
    public sealed class UserTokenRepository : IUserTokenRepository
    {
        private readonly IDbConnection _connection;

        public UserTokenRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void Add(UserToken userToken) =>
            _connection.Execute(
                "INSERT INTO [UserToken] " +
                "([Token], [UserID]) " +
                "VALUES " +
                "(@Token, @UserId)",
                userToken
            );

        public void Update(UserToken userToken) =>
            _connection.Execute(
                "UPDATE [UserToken] " +
                "SET [Token] = @Token " +
                "WHERE ID = @Id",
                userToken
            );

        public UserToken GetByUserIdIfExists(string userId) =>
            _connection.Query<UserToken>(
                "SELECT TOP(1) * " +
                "FROM [UserToken] " +
                "WHERE [UserToken].UserID = @UserId",
                new { UserId = userId }
            ).SingleOrDefault();

        public void DeleteUserTokenByUserId(string userId) =>
            _connection.Execute(
                "DELETE FROM [UserToken] " +
                "WHERE [UserToken].UserID = @UserId",
                new { UserId = userId }
            );
    }
}
