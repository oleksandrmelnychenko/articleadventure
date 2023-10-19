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
    public class MainArticleRepository : IMainArticleRepository
    {
        private readonly IDbConnection _connection;
        public MainArticleRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public long AddMainArticle(MainArticle blog)
            => _connection.Query<long>("INSERT INTO [MainArticle] " +
            "([Title], [Description] ,[InfromationArticle] ,[Price] ,[ImageUrl] ,[UserId] ,[Updated] ) " +
            "VALUES " +
            "(@Title, @Description, @InfromationArticle , @Price, @ImageUrl, @UserId, GETUTCDATE());" +
            "SELECT SCOPE_IDENTITY()", blog
            ).Single();

        public List<MainArticle> GetAllArticlesFilterSupTag(List<MainArticleTags> supTags)
        {
            List<MainArticle> mainArticles = new List<MainArticle>();

            List<long> IdMainArticleTags = supTags.Select(x => x.MainArticleId).ToList();
            _connection.Query<MainArticle, AuthorArticle, MainArticleTags, SupTag, UserProfile, MainArticle>("SELECT mainArticle.*, " +
            "authorArticle.*, " +
            "articleTags.*, " +
            "supTags.*, " +
            "userProfile.* " +
            "FROM [ArticleAdventure].[dbo].[MainArticle] AS mainArticle " +
            "LEFT JOIN [ArticleAdventure].[dbo].[AuthorArticle] AS authorArticle " +
            "ON mainArticle.ID = authorArticle.MainArticleId " +
            "LEFT JOIN [ArticleAdventure].[dbo].[ArticleTags] AS articleTags " +
            "ON mainArticle.ID = articleTags.MainArticleId " +
            "LEFT JOIN [ArticleAdventure].[dbo].[SubTags] AS supTags " +
            "ON supTags.ID = articleTags.SupTagId " +
            "LEFT JOIN [ArticleAdventure].[dbo].[UserProfile] as userProfile " +
            "ON userProfile.ID = mainArticle.UserId " +
            "WHERE mainArticle.Deleted = 0 " +
            "AND authorArticle.Deleted = 0 " +
            "AND articleTags.Deleted = 0 " +
            "AND supTags.Deleted = 0",
                (mainArticle, article, articleMainTag, supTag, userProfile) =>
                {
                    if (IdMainArticleTags.Any(x => x.Equals(articleMainTag.MainArticleId)))
                    {
                        if (mainArticles.Any(c => c.Id.Equals(mainArticle.Id)))
                        {
                            mainArticle = mainArticles.First(c => c.Id.Equals(mainArticle.Id));
                        }
                        else
                        {
                            mainArticle.UserProfile = userProfile;
                            mainArticles.Add(mainArticle);
                        }

                        if (mainArticles.Any(x => x.Articles.Any(x => x.Id.Equals(article.Id))))
                        {
                            article = mainArticles.SelectMany(artics => artics.Articles)
                            .First(c => c.Id.Equals(article.Id));
                        }
                        else
                        {

                            mainArticles.Where(art => article.MainArticleId == art.Id)
                            .ToList()
                            .ForEach(art => art.Articles.Add(article));
                        }

                        if (mainArticles.Any(x => x.ArticleTags.Any(x => x.Id.Equals(articleMainTag.Id))))
                        {
                            articleMainTag = mainArticles.SelectMany(x => x.ArticleTags)
                            .First(c => c.Id.Equals(articleMainTag.Id));
                        }
                        else
                        {
                            mainArticles.Where(a => articleMainTag.MainArticleId == a.Id)
                            .ToList()
                            .ForEach(x => x.ArticleTags.Add(articleMainTag));

                            foreach (var art in mainArticles)
                            {
                                art.ArticleTags.Where(item => item.SupTagId == supTag.Id)
                                              .ToList()
                                              .ForEach(item => item.SupTag = supTag);
                            }
                        }
                    }

                    return mainArticle;
                }).ToList();
            return mainArticles;
        }


        public List<MainArticle> GetAllArticles()
        {
            List<MainArticle> mainArticles = new List<MainArticle>();

            _connection.Query<MainArticle, AuthorArticle, MainArticleTags, SupTag, UserProfile, MainArticle>("SELECT mainArticle.*, " +
            "authorArticle.*, " +
            "articleTags.*, " +
            "supTags.*," +
            "userProfile.* " +
            "FROM [ArticleAdventure].[dbo].[MainArticle] AS mainArticle " +
            "LEFT JOIN [ArticleAdventure].[dbo].[AuthorArticle] AS authorArticle " +
            "ON mainArticle.ID = authorArticle.MainArticleId " +
            "LEFT JOIN [ArticleAdventure].[dbo].[ArticleTags] AS articleTags " +
            "ON mainArticle.ID = articleTags.MainArticleId " +
            "LEFT JOIN [ArticleAdventure].[dbo].[SubTags] AS supTags " +
            "ON supTags.ID = articleTags.SupTagId " +
            "LEFT JOIN [ArticleAdventure].[dbo].[UserProfile] as userProfile " +
            "ON userProfile.ID = mainArticle.UserId " +
            "WHERE mainArticle.Deleted = 0 " +
            "AND authorArticle.Deleted = 0 " +
            "AND articleTags.Deleted = 0 " +
            "AND supTags.Deleted = 0",
                (mainArticle, article, articleMainTag, supTag, userProfile) =>
                {
                    if (mainArticles.Any(c => c.Id.Equals(mainArticle.Id)))
                    {
                        mainArticle = mainArticles.First(c => c.Id.Equals(mainArticle.Id));
                    }
                    else
                    {
                        mainArticle.UserProfile = userProfile;
                        mainArticles.Add(mainArticle);
                    }

                    if (mainArticles.Any(x => x.Articles.Any(x => x.Id.Equals(article.Id))))
                    {
                        article = mainArticles.SelectMany(artics => artics.Articles)
                        .First(c => c.Id.Equals(article.Id));
                    }
                    else
                    {

                        mainArticles.Where(art => article.MainArticleId == art.Id)
                        .ToList()
                        .ForEach(art => art.Articles.Add(article));
                    }

                    if (mainArticles.Any(x => x.ArticleTags.Any(x => x.Id.Equals(articleMainTag.Id))))
                    {
                        articleMainTag = mainArticles.SelectMany(x => x.ArticleTags)
                        .First(c => c.Id.Equals(articleMainTag.Id));
                    }
                    else
                    {
                        mainArticles.Where(a => articleMainTag.MainArticleId == a.Id)
                        .ToList()
                        .ForEach(x => x.ArticleTags.Add(articleMainTag));

                        foreach (var art in mainArticles)
                        {
                            art.ArticleTags.Where(item => item.SupTagId == supTag.Id)
                                          .ToList()
                                          .ForEach(item => item.SupTag = supTag);
                        }
                    }
                    return mainArticle;
                }).ToList();
            return mainArticles;
        }

        public MainArticle GetArticle(long id)
        {
            MainArticle articleMain = new MainArticle();

            _connection.Query<MainArticle, AuthorArticle, MainArticleTags, SupTag, UserProfile, MainArticle>("SELECT mainArticle.*, " +
           "authorArticle.*, " +
           "articleTags.*, " +
           "supTags.*, " +
           "userProfile.* " +
           "FROM [ArticleAdventure].[dbo].[MainArticle] AS mainArticle " +
           "LEFT JOIN [ArticleAdventure].[dbo].[AuthorArticle] AS authorArticle " +
           "ON mainArticle.ID = authorArticle.MainArticleId " +
           "LEFT JOIN [ArticleAdventure].[dbo].[ArticleTags] AS articleTags " +
           "ON mainArticle.ID = articleTags.MainArticleId " +
           "LEFT JOIN [ArticleAdventure].[dbo].[SubTags] AS supTags " +
           "ON supTags.ID = articleTags.SupTagId " +
           "LEFT JOIN [ArticleAdventure].[dbo].[UserProfile] as userProfile " +
           "ON userProfile.ID = mainArticle.UserId " +
           "WHERE mainArticle.Deleted = 0 " +
           "AND authorArticle.Deleted = 0 " +
           "AND articleTags.Deleted = 0 " +
           "AND supTags.Deleted = 0" +
           "AND mainArticle.ID = @ID",
               (mainArticle, article, articleMainTag, supTag, userProfile) =>
               {
                   if (articleMain.Id.Equals(mainArticle.Id))
                   {
                       mainArticle = articleMain;
                   }
                   else
                   {
                       mainArticle.UserProfile = userProfile;
                       articleMain = mainArticle;
                   }

                   if (articleMain.Articles.Any(x => x.Id.Equals(article.Id)))
                   {
                       article = articleMain.Articles.First(c => c.Id.Equals(article.Id));
                   }
                   else
                   {
                       mainArticle.Articles.Add(article);
                   }

                   if (articleMain.ArticleTags.Any(x => x.Id.Equals(articleMainTag.Id)))
                   {
                       articleMainTag = articleMain.ArticleTags.First(c => c.Id.Equals(articleMainTag.Id));
                   }
                   else
                   {
                       mainArticle.ArticleTags.Add(articleMainTag);
                       mainArticle.ArticleTags.Where(item => item.SupTagId == supTag.Id)
                                         .ToList()
                                         .ForEach(item => item.SupTag = supTag);
                   }
                   return mainArticle;
               },
               new { ID = id }).ToList();

            return articleMain;

        }

        public AuthorArticle GetSupArticle(Guid netUid) => _connection.Query<AuthorArticle>("SELECT * FROM [AuthorArticle] AS AuthorArticle " +
                "WHERE AuthorArticle.NetUid = @NetUid"
            , new { NetUid = netUid }).Single();
        public MainArticle GetArticle(Guid netUid)
        {
            MainArticle articleMain = new MainArticle();

            _connection.Query<MainArticle, AuthorArticle, MainArticleTags, SupTag,UserProfile, MainArticle>("SELECT mainArticle.*, " +
           "authorArticle.*, " +
           "articleTags.*, " +
           "supTags.*, " +
           "userProfile.* " +
           "FROM [ArticleAdventure].[dbo].[MainArticle] AS mainArticle " +
           "LEFT JOIN [ArticleAdventure].[dbo].[AuthorArticle] AS authorArticle " +
           "ON mainArticle.ID = authorArticle.MainArticleId " +
           "LEFT JOIN [ArticleAdventure].[dbo].[ArticleTags] AS articleTags " +
           "ON mainArticle.ID = articleTags.MainArticleId " +
           "LEFT JOIN [ArticleAdventure].[dbo].[SubTags] AS supTags " +
           "ON supTags.ID = articleTags.SupTagId " +
           "LEFT JOIN [ArticleAdventure].[dbo].[UserProfile] as userProfile " +
           "ON userProfile.ID = mainArticle.UserId " +
           "WHERE mainArticle.Deleted = 0 " +
           "AND authorArticle.Deleted = 0 " +
           "AND articleTags.Deleted = 0 " +
           "AND supTags.Deleted = 0" +
           "AND mainArticle.NetUID = @NetUid",
               (mainArticle, article, articleMainTag, supTag, userProfile) =>
               {
                   if (articleMain.Id.Equals(mainArticle.Id))
                   {
                       mainArticle = articleMain;
                   }
                   else
                   {
                       mainArticle.UserProfile = userProfile;
                       articleMain = mainArticle;
                   }

                   if (articleMain.Articles.Any(x => x.Id.Equals(article.Id)))
                   {
                       article = articleMain.Articles.First(c => c.Id.Equals(article.Id));
                   }
                   else
                   {
                       mainArticle.Articles.Add(article);
                   }

                   if (articleMain.ArticleTags.Any(x => x.Id.Equals(articleMainTag.Id)))
                   {
                       articleMainTag = articleMain.ArticleTags.First(c => c.Id.Equals(articleMainTag.Id));
                   }
                   else
                   {
                       mainArticle.ArticleTags.Add(articleMainTag);
                       mainArticle.ArticleTags.Where(item => item.SupTagId == supTag.Id)
                                         .ToList()
                                         .ForEach(item => item.SupTag = supTag);
                   }
                   return mainArticle;
               },
               new { NetUid = netUid }).ToList();

            return articleMain;

        }



        public void RemoveMainArticle(Guid netUid)
            => _connection.Execute("UPDATE [ArticleAdventure].[dbo].[MainArticle] " +
                "SET [Deleted] = 1 " +
                "WHERE [ArticleAdventure].[dbo].[MainArticle].NetUID = @NetUID",
                new { NetUID = netUid });

        public void UpdateMainArticle(MainArticle blog)
            => _connection.Execute("Update [MainArticle] " +
                "SET [Title] = @Title, [Description] = @Description,[Image] = @Image " +
                ",[ImageUrl] = @ImageUrl ,[Price] = @Price, [WebImageUrl] = @WebImageUrl ,[InfromationArticle] = @InfromationArticle " +
                ",[Updated] = GETUTCDATE() " +
                $"WHERE [MainArticle].[NetUid] = @NetUid ",
                blog);


    }
}
