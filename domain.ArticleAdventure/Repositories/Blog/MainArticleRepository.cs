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
            "([Title], [Description] ,[InfromationArticle] ,[Price] ,[Updated] ) " +
            "VALUES " +
            "(@Title, @Description" +
            ", @InfromationArticle , @Price, GETUTCDATE());" +
            "SELECT SCOPE_IDENTITY()", blog
            ).Single();

        public List<MainArticle> GetAllArticles()
        {
            List<MainArticle> mainArticles = new List<MainArticle>();
            List<AuthorArticle> supArticles = new List<AuthorArticle>();
            List<MainArticleTags> articleTags = new List<MainArticleTags>();

            List<MainTag> mainTags = new List<MainTag>();

            //add getArticle

            //_connection.Query<MainArticle, AuthorArticle, MainArticleTags, MainArticle>("SELECT mainArticles.*, subTags.* " +
            //       "FROM [MainTags] AS mainTag " +
            //       "LEFT JOIN [SubTags] AS subTags " +
            //       "ON mainTag.Id = subTags.IdMainTag " +
            //       "AND subTags.Deleted = 0 " +
            //       "WHERE mainTag.Deleted = 0 ",
            //    (mainArticle, article,articleMainTag) =>
            //    {
            //    }).ToList();
            return null;
            //return mainTags;
        }

        public MainArticle GetArticle(Guid netUid)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid netUid)
        {
            throw new NotImplementedException();
        }

        public void Update(MainArticle blog)
        {
            throw new NotImplementedException();
        }
    }
}
