﻿using Dapper;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using domain.ArticleAdventure.Repositories.Identity.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Identity
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
            => _connection.Query<UserProfile>(
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

        public void Update(UserProfile userProfile) =>
            _connection.Execute(
                "UPDATE [UserProfile] " +
                "SET [UserName] = @UserName, [Email] = @Email, [GrantAdministrativePermissions] = @GrantAdministrativePermissions, " +
                "[Updated] = GETUTCDATE() " +
                "WHERE ID = @Id",
                userProfile
            );

        public void UpdateAccountInformation(UserProfile userProfile) =>
           _connection.Execute(
               "UPDATE [UserProfile] " +
               "SET [UserName] = @UserName, [InformationAccount] = @InformationAccount, [SurName] = @SurName, " +
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

        public FavoriteArticle GetFavoriteArticle(long userId, long mainArticleId)
            => _connection.Query<FavoriteArticle>(
                  "SELECT * FROM [FavoriteArticles] AS favoriteArticles " +
                  "WHERE favoriteArticles.MainArticleId = @MainArticleId " +
                  "AND favoriteArticles.UserId = @UserId ",
               new { UserId = userId, MainArticleId = mainArticleId }).SingleOrDefault();

        public long SetFavoriteArticle(long mainArticleId, long userId) =>
         _connection.Query<long>(
             "INSERT INTO [FavoriteArticles] " +
             "([MainArticleId], [UserId], [Updated]) " +
             "VALUES " +
             "(@MainArticleId, @UserId, GETUTCDATE()); " +
             "SELECT SCOPE_IDENTITY()",
             new
             {
                 MainArticleId = mainArticleId,
                 UserId = userId
             }
         ).Single();

        public List<FavoriteArticle> GetAllFavoriteArticle(long userId)
        {

            List<FavoriteArticle> favoriteArticles = new List<FavoriteArticle>();

            _connection.Query<FavoriteArticle, MainArticle, FavoriteArticle>(
                  "SELECT favoriteArticles.*, " +
                  "mainArticle.* " +
                  "FROM [ArticleAdventure].[dbo].[FavoriteArticles] as favoriteArticles " +
                  "LEFT JOIN [ArticleAdventure].[dbo].[MainArticle] as mainArticle " +
                  "On mainArticle.ID = favoriteArticles.MainArticleId " +
                  "where favoriteArticles.UserId = @UserId ",
               (favoriteArticle, mainArticle) =>
               {
                   if (favoriteArticles.Any(x => x.Id.Equals(favoriteArticle.Id)))
                   {
                       favoriteArticle = favoriteArticles.First(x => x.Id.Equals(favoriteArticle.Id));
                   }
                   else
                   {
                       favoriteArticles.Add(favoriteArticle);
                   }

                   if (favoriteArticles.Any(x => x.MainArticle.Id.Equals(mainArticle.Id)))
                   {
                       mainArticle = favoriteArticles.First(x => x.MainArticle.Id.Equals(mainArticle.Id)).MainArticle;
                   }
                   else
                   {
                       favoriteArticles.Where(x => x.MainArticleId == mainArticle.Id).ToList()
                       .ForEach(x => x.MainArticle = mainArticle);
                   }

                   return favoriteArticle;
               },
               new { UserId = userId }).ToList();



            return favoriteArticles;
        }


        public long RemoveFavoriteArticle(Guid netUidFavoriteArticle)
            => _connection.Execute("DELETE FROM [ArticleAdventure].[dbo].[FavoriteArticles] " +
                "WHERE FavoriteArticles.NetUID = @NetUID",
                new { NetUID = netUidFavoriteArticle });


    }
}
