using Dapper;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Blog
{
    public sealed class UserProfileRepository : IUserProfileRepository
    {
        private readonly IDbConnection _connection;

        public UserProfileRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public long Add(UserProfile userProfile) =>
         _connection.Query<long>(
             "INSERT INTO [UserProfile] " +
             "([UserName], [Email], [GrantAdministrativePermissions], [Updated]) " +
             "VALUES " +
             "(@UserName, @Email, @GrantAdministrativePermissions, GETUTCDATE()); " +
             "SELECT SCOPE_IDENTITY()",
             userProfile
         ).Single();

        public UserProfile Get(long id) =>
            _connection.Query<UserProfile>(
                "SELECT * " +
                "FROM [UserProfile] " +
                "WHERE [UserProfile].ID = @Id",
                new { Id = id }
            ).SingleOrDefault();

        public UserProfile Get(Guid netId) =>
            _connection.Query<UserProfile>(
                "SELECT * " +
                "FROM [UserProfile] " +
                "WHERE [UserProfile].NetUID = @NetId",
                new { NetId = netId }
            ).SingleOrDefault();

        public UserProfile Get(string userName, string email) =>
            _connection.Query<UserProfile>(
                "SELECT * " +
                "FROM [UserProfile] " +
                "WHERE [UserProfile].UserName = @UserName " +
                "AND [UserProfile].Email = @Email " +
                "AND [UserProfile].Deleted = 0",
                new { UserName = userName, Email = email }
            ).SingleOrDefault();


        public UserProfile Get(string email) =>
            _connection.Query<UserProfile>(
                "SELECT * " +
                "FROM [UserProfile] " +
                "WHERE [UserProfile].Email = @Email " +
                "AND [UserProfile].Deleted = 0",
                new { Email = email }
            ).SingleOrDefault();

        public IEnumerable<UserProfile> GetAllFiltered(string value, int limit, int offset)
        {
            return _connection.Query<UserProfile>(
                        ";WITH [Filter_CTE] " +
                        "AS ( " +
                        "SELECT [UserProfile].ID " +
                        ", ROW_NUMBER() OVER(ORDER BY [UserProfile].UserName) [RowNumber] " +
                        "FROM [UserProfile] " +
                        "WHERE [UserProfile].Deleted = 0 " +
                        "AND ( " +
                        "PATINDEX( N'%' + @Value + N'%', [UserProfile].Email) <> 0 " +
                        "OR " +
                        "PATINDEX( N'%' + @Value + N'%', [UserProfile].UserName) <> 0 " +
                        ") " +
                        ") " +
                        "SELECT [UserProfile].* " +
                        "FROM [UserProfile] " +
                        "LEFT JOIN [Filter_CTE] AS [Filter] " +
                        "ON [Filter].ID = [UserProfile].ID " +
                        "WHERE [Filter].RowNumber > @Offset " +
                        "AND [Filter].RowNumber <= @Limit + @Offset",
                        new { Value = value, Limit = limit, Offset = offset }
                    );
        }

        public void Update(UserProfile userProfile) =>
            _connection.Execute(
                "UPDATE [UserProfile] " +
                "SET [UserName] = @UserName, [Email] = @Email, [GrantAdministrativePermissions] = @GrantAdministrativePermissions, " +
                "[Updated] = GETUTCDATE() " +
                "WHERE ID = @Id",
                userProfile
            );

        public void Remove(long id) =>
            _connection.Execute(
                "UPDATE [UserProfile] " +
                "SET Deleted = 1, Updated = GETUTCDATE() " +
                "WHERE ID = @Id",
                new { Id = id }
            );

        public void Remove(Guid netId) =>
            _connection.Execute(
                "UPDATE [UserProfile] " +
                "SET Deleted = 1, Updated = GETUTCDATE() " +
                "WHERE NetUID = @NetId",
                new { NetId = netId }
            );
    }
}
