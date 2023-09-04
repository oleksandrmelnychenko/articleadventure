using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Repositories.Blog;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using domain.ArticleAdventure.Repositories.Stripe.Contracts;
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
        private readonly IStripeRepositoryFactory _stripeRepositoryFactory;
        public MainArticleService(IDbConnectionFactory connectionFactory,
            IMainArticleRepositoryFactory mainArticleRepositoryFactory,
            IMainArticleTagsFactory mainArticleTagsFactory,
            IArticleRepositoryFactory articleRepositoryFactory, IStripeRepositoryFactory stripeRepositoryFactory) : base(connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _mainRepositoryFactory = mainArticleRepositoryFactory;
            _mainArticleTagsFactory = mainArticleTagsFactory;
            _articleRepositoryFactory = articleRepositoryFactory;
            _stripeRepositoryFactory = stripeRepositoryFactory;
        }

        public Task<long> AddArticle(MainArticle article, IFormFile filePhotoMainArticle)
        {
            return Task.Run(async () =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    string exention = ".png";

                    if (filePhotoMainArticle != null)
                    {
                        string pathLogo = Path.Combine(ArticleAdventureFolderManager.GetFilesFolderPath(), ArticleAdventureFolderManager.GetStaticImageFolder(), filePhotoMainArticle.FileName + exention);
                        article.ImageUrl = Path.Combine(ArticleAdventureFolderManager.GetStaticServerUrlImageFolder(), filePhotoMainArticle.FileName + exention);

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
        public Task<MainArticle> GetArticle(long id) =>
            Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _mainRepositoryFactory.New(connection).GetArticle(id);
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

        public Task Update(MainArticle article, IFormFile filePhotoMainArticle) =>
            Task.Run(async () =>
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                string exention = ".png";

                if (filePhotoMainArticle != null)
                {
                    string pathLogo = Path.Combine(ArticleAdventureFolderManager.GetFilesFolderPath(), ArticleAdventureFolderManager.GetStaticImageFolder(), filePhotoMainArticle.FileName + exention);
                    article.ImageUrl = Path.Combine(ArticleAdventureFolderManager.GetStaticServerUrlImageFolder(), filePhotoMainArticle.FileName + exention);

                    using (var stream = new FileStream(pathLogo, FileMode.Create))
                    {
                        await filePhotoMainArticle.CopyToAsync(stream);
                    }
                }
                var mainArticle = _mainRepositoryFactory.New(connection).GetArticle(article.NetUid);

                _mainRepositoryFactory.New(connection).UpdateMainArticle(article);
                _mainArticleTagsFactory.New(connection).RemoveMainTag(article.Id);

                foreach (var tag in article.ArticleTags)
                {
                    _mainArticleTagsFactory.New(connection).AddMainTag(tag);
                }
            }
        });

        public Task<List<MainArticle>> GetAllArticlesUser(long idUser) =>
            Task.Run(async () =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    List<MainArticle> mainArticles = new List<MainArticle>();
                    List<AuthorArticle> authorArticles = new List<AuthorArticle>();
                    List<AuthorArticle> filterAticle = new List<AuthorArticle>();
                    var stripeRepository = _stripeRepositoryFactory.New(connection);
                    var stripePayments = stripeRepository.GetPaymentIUserdMainArticle(idUser);


                    foreach (var payment in stripePayments)
                    {
                        var mainArticle = _mainRepositoryFactory.New(connection).GetArticle(payment.MainArticleId);
                        if (!mainArticles.Any(x=>x.Id == mainArticle.Id))
                        {
                            mainArticles.Add(mainArticle);
                        }
                        
                    }

                    authorArticles.AddRange(mainArticles.SelectMany(x => x.Articles));


                    mainArticles.ForEach(x => x.Articles.Clear());

                    foreach (var article in authorArticles)
                    {
                        if (stripePayments.Any(x=>x.SupArticleId==article.Id))
                        {
                            filterAticle.Add(article);
                        }
                    }

                    foreach (var article in mainArticles)
                    {
                        if (stripePayments.Any(x=>x.MainArticleId==article.Id))
                        {

                        }
                    }

                    //foreach (var article in mainArticles)
                    //{
                    //    foreach (var filterArt in filterAticle)
                    //    {
                    //        if (stripePayments.Any(x=>x.SupArticleId== filterArt.Id))
                    //        {
                    //            article.Articles.Add(filterArt);
                    //        }
                    //    }
                    //}

                    return mainArticles;
                }
            });
    }
} 
