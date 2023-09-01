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
        //,[Image] ,[ImageUrl] ,[WebImageUrl]
        //, @Image, @ImageUrl, @WebImageUrl
        public long AddMainArticle(MainArticle blog)
            => _connection.Query<long>("INSERT INTO [MainArticle] " +
            "([Title], [Description] ,[InfromationArticle] ,[Price] ,[ImageUrl] ,[Updated] ) " +
            "VALUES " +
            "(@Title, @Description" +
            ", @InfromationArticle , @Price, @ImageUrl, GETUTCDATE());" +
            "SELECT SCOPE_IDENTITY()", blog
            ).Single();

        public List<MainArticle> GetAllArticles()
        {
            List<MainArticle> mainArticles = new List<MainArticle>();
            //add getArticle

            _connection.Query<MainArticle, AuthorArticle, MainArticleTags, SupTag, MainArticle>("SELECT mainArticle.*, " +
            "authorArticle.*, " +
            "articleTags.*, " +
            "supTags.* " +
            "FROM [ArticleAdventure].[dbo].[MainArticle] AS mainArticle " +
            "LEFT JOIN [ArticleAdventure].[dbo].[AuthorArticle] AS authorArticle " +
            "ON mainArticle.ID = authorArticle.MainArticleId " +
            "LEFT JOIN [ArticleAdventure].[dbo].[ArticleTags] AS articleTags " +
            "ON mainArticle.ID = articleTags.MainArticleId " +
            "LEFT JOIN [ArticleAdventure].[dbo].[SubTags] AS supTags " +
            "ON supTags.ID = articleTags.SupTagId " +
            "WHERE mainArticle.Deleted = 0 " +
            "AND authorArticle.Deleted = 0 " +
            "AND articleTags.Deleted = 0 " ,
                (mainArticle, article, articleMainTag, supTag) =>
                {
                    if (mainArticles.Any(c => c.Id.Equals(mainArticle.Id)))
                    {
                        mainArticle = mainArticles.First(c => c.Id.Equals(mainArticle.Id));
                    }
                    else
                    {
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
            //return mainTags;
        }

        public MainArticle GetArticle(long id)
        {
            MainArticle articleMain = new MainArticle();

            _connection.Query<MainArticle, AuthorArticle, MainArticleTags, SupTag, MainArticle>("SELECT mainArticle.*, " +
           "authorArticle.*, " +
           "articleTags.*, " +
           "supTags.* " +
           "FROM [ArticleAdventure].[dbo].[MainArticle] AS mainArticle " +
           "LEFT JOIN [ArticleAdventure].[dbo].[AuthorArticle] AS authorArticle " +
           "ON mainArticle.ID = authorArticle.MainArticleId " +
           "LEFT JOIN [ArticleAdventure].[dbo].[ArticleTags] AS articleTags " +
           "ON mainArticle.ID = articleTags.MainArticleId " +
           "LEFT JOIN [ArticleAdventure].[dbo].[SubTags] AS supTags " +
           "ON supTags.ID = articleTags.SupTagId " +
           "WHERE mainArticle.Deleted = 0 " +
           "AND authorArticle.Deleted = 0 " +
           "AND articleTags.Deleted = 0 " +
           "AND mainArticle.ID = @ID",
               (mainArticle, article, articleMainTag, supTag) =>
               {
                   if (articleMain.Id.Equals(mainArticle.Id))
                   {
                       mainArticle = articleMain;
                   }
                   else
                   {
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

        public MainArticle GetArticle(Guid netUid)
        {
            MainArticle articleMain = new MainArticle();

            _connection.Query<MainArticle, AuthorArticle, MainArticleTags, SupTag, MainArticle>("SELECT mainArticle.*, " +
           "authorArticle.*, " +
           "articleTags.*, " +
           "supTags.* " +
           "FROM [ArticleAdventure].[dbo].[MainArticle] AS mainArticle " +
           "LEFT JOIN [ArticleAdventure].[dbo].[AuthorArticle] AS authorArticle " +
           "ON mainArticle.ID = authorArticle.MainArticleId " +
           "LEFT JOIN [ArticleAdventure].[dbo].[ArticleTags] AS articleTags " +
           "ON mainArticle.ID = articleTags.MainArticleId " +
           "LEFT JOIN [ArticleAdventure].[dbo].[SubTags] AS supTags " +
           "ON supTags.ID = articleTags.SupTagId " +
           "WHERE mainArticle.Deleted = 0 " +
           "AND authorArticle.Deleted = 0 " +
           "AND articleTags.Deleted = 0 " +
           "AND mainArticle.NetUID = @NetUid",
               (mainArticle, article, articleMainTag, supTag) =>
               {
                   if (articleMain.Id.Equals(mainArticle.Id))
                   {
                       mainArticle = articleMain;
                   }
                   else
                   {
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
            => _connection.Execute("DELETE FROM [ArticleAdventure].[dbo].[MainArticle] " +
                "WHERE MainArticle.NetUID = @NetUID",
                new { NetUID = netUid });

        public void UpdateMainArticle(MainArticle blog) 
            => _connection.Execute("Update [MainArticle] " +
                "SET [Title] = @Title, [Description] = @Description,[Image] = @Image " +
                ",[ImageUrl] = @ImageUrl ,[WebImageUrl] = @WebImageUrl ,[InfromationArticle] = @InfromationArticle " +
                ",[Updated] = GETUTCDATE() " +
                $"WHERE [MainArticle].[NetUid] = @NetUid ",
                blog);
    }
}
