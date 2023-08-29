using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Repositories.Blog;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using domain.ArticleAdventure.Repositories.Tag.Contracts;
using Microsoft.AspNetCore.Http;
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

        public Task<long> AddArticle(MainArticle article,IFormFile filePhotoMainArticle)
        {
            return Task.Run(async () =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    string exention = ".png";

                    if (filePhotoMainArticle!= null)
                    {
                        string pathLogo = Path.Combine(ArticleAdventureFolderManager.GetFilesFolderPath(), ArticleAdventureFolderManager.GetStaticImageFolder(), filePhotoMainArticle.FileName + exention);
                        article.ImageUrl = Path.Combine( ArticleAdventureFolderManager.GetStaticServerUrlImageFolder(), filePhotoMainArticle.FileName + exention);

                        using (var stream = new FileStream(pathLogo, FileMode.Create))
                        {
                            await filePhotoMainArticle.CopyToAsync(stream);
                        }
                    }
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
                }
            });
        }

        public Task<MainArticle> GetArticle(Guid netUid) =>
            Task.Run(() =>
             {
                 using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                 {
                     return _mainRepositoryFactory.New(connection).GetArticle(netUid);
                 }
             });
        

        public Task<List<MainArticle>> GetAllArticles() => 
            Task.Run(() =>
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                return _mainRepositoryFactory.New(connection).GetAllArticles();
            }
        });

        public Task Remove(Guid netUid) => 
            Task.Run(() =>
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                _mainRepositoryFactory.New(connection).RemoveMainArticle(netUid);
            }
        });

        public Task Update(MainArticle Article) => 
            Task.Run(() =>
        {

            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
               var mainArticle = _mainRepositoryFactory.New(connection).GetArticle(Article.NetUid);

                _mainRepositoryFactory.New(connection).UpdateMainArticle(Article);
                _mainArticleTagsFactory.New(connection).RemoveMainTag(Article.Id);

                foreach (var tag in Article.ArticleTags)
                {
                        _mainArticleTagsFactory.New(connection).AddMainTag(tag);
                }

            }
        });
    }
}
