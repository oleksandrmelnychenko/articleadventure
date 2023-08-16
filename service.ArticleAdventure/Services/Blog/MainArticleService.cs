using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using domain.ArticleAdventure.Repositories.Tag.Contracts;
using service.ArticleAdventure.Services.Blog.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services.Blog
{
    public class MainArticleService : BaseService, IMainArticleService
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IArticleRepositoryFactory _articleRepositoryFactory;
        private readonly IMainArticleRepositoryFactory _mainRepositoryFactory;
        private readonly IMainArticleTagsFactory _mainArticleTagsFactory;
        public MainArticleService(IDbConnectionFactory connectionFactory,
            IMainArticleRepositoryFactory mainArticleRepositoryFactory,
            IMainArticleTagsFactory mainArticleTagsFactory,
            IArticleRepositoryFactory articleRepositoryFactory) : base(connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _mainRepositoryFactory = mainArticleRepositoryFactory;
            _mainArticleTagsFactory = mainArticleTagsFactory;
            _articleRepositoryFactory = articleRepositoryFactory;
        }

        public Task<long> AddArticle(MainArticle article)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    var idMainArticle = _mainRepositoryFactory.New(connection).AddMainArticle(article);

                    foreach (var tag in article.ArticleTags)
                    {
                        tag.MainArticleId = idMainArticle;
                        _mainArticleTagsFactory.New(connection).AddMainTag(tag);
                    }
                    foreach (var item in article.Articles)
                    {
                        item.MainArticleId = idMainArticle;
                         _articleRepositoryFactory.New(connection).AddArticle(item);

                    }
                    return idMainArticle;
                    //_mainRepositoryFactory.New(connection)
                }
            });
        }

        public Task<MainArticle> GetArticle(Guid netUid)
        {
            //return Task.Run(() =>
            // {
            //     using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            //     {
            //         return _blogRepositoryFactory.New(connection).GetArticle(netUid);
            //     }
            // });
            return null;
        }

        public Task<List<MainArticle>> GetAllArticles()
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _mainRepositoryFactory.New(connection).GetAllArticles();
                }
            });
            return null;
        }

        public Task Remove(Guid netUid)
        //=> Task.Run(() => {

        //    using (IDbConnection connection = _connectionFactory.NewSqlConnection())
        //    {
        //        _blogRepositoryFactory.New(connection).Remove(netUid);
        //    }
        //});
        { return null; }

        public Task Update(MainArticle Article)
        //=> Task.Run(() => {

        //    using (IDbConnection connection = _connectionFactory.NewSqlConnection())
        //    {
        //        _blogRepositoryFactory.New(connection).Update(Article);
        //    }
        //});
        { return null; }
    }
}
