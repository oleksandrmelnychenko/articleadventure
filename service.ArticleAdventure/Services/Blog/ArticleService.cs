using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using service.ArticleAdventure.Services.Blog.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services.Blog
{
    public class ArticleService : BaseService , IArticleService
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IArticleRepositoryFactory _ArticleRepositoryFactory;
        public ArticleService(IDbConnectionFactory connectionFactory,
            IArticleRepositoryFactory blogRepositoryFactory) : base(connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _ArticleRepositoryFactory = blogRepositoryFactory;
        }
         
        public Task<long> AddArticle(AuthorArticle blog)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _ArticleRepositoryFactory.New(connection).AddArticle(blog);
                }
            });
                
        }

        public Task<AuthorArticle> GetArticle(Guid netUid)
            => Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _ArticleRepositoryFactory.New(connection).GetArticle(netUid);
                }
            });


        public Task<List<AuthorArticle>> GetAllArticles()
        {
            return Task.Run(() => 
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _ArticleRepositoryFactory.New(connection).GetAllArticles();
                }
            });
        }

        public Task Remove(Guid netUid) 
            => Task.Run(()=> {

                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                     _ArticleRepositoryFactory.New(connection).Remove(netUid);
                }
            });

        public Task Update(AuthorArticle Article)
          => Task.Run(() => {

              using (IDbConnection connection = _connectionFactory.NewSqlConnection())
              {
                   _ArticleRepositoryFactory.New(connection).Update(Article);
              }
          });

    }
}
