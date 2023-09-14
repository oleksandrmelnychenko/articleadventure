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

        public Task<AuthorArticle> GetSupArticle(Guid netUid) =>
          Task.Run(() =>
          {
              using (IDbConnection connection = _connectionFactory.NewSqlConnection())
              {
                  return _mainRepositoryFactory.New(connection).GetSupArticle(netUid);
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

        public Task<List<MainArticle>> GetAllArticlesFilterSupTags(List<MainArticleTags> mainTags) =>
            Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _mainRepositoryFactory.New(connection).GetAllArticlesFilterSupTag(mainTags);
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

        public Task<long> Update(MainArticle article, IFormFile filePhotoMainArticle) =>
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
                return mainArticle.Id;
            }
        });

        public Task<List<MainArticle>> GetAllArticlesUser(long idUser) =>
            Task.Run(async () =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    List<MainArticle> mainArticles = new List<MainArticle>();
                    var stripeRepository = _stripeRepositoryFactory.New(connection);
                    List<StripePayment> stripePayments = stripeRepository.GetPaymentIUserdMainArticle(idUser);

                    foreach (var stripePayment in stripePayments)
                    {
                        if (mainArticles.Any(x => x.Id.Equals(stripePayment.MainArticleId)))
                        {
                            mainArticles.Where(x => x.Id == stripePayment.MainArticleId).ToList().ForEach(x => x.Articles.Add(stripePayment.SupArticle));
                        }
                        else
                        {
                            mainArticles.Add(stripePayment.MainArticle);
                            mainArticles.Where(x => x.Id == stripePayment.MainArticleId).ToList().ForEach(x => x.Articles.Add(stripePayment.SupArticle));
                        }
                    }

                    return mainArticles;
                }
            });

        public Task<List<StripePayment>> GetAllPaymentArticleUser(long idUser) =>
            Task.Run(async () =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    List<MainArticle> mainArticles = new List<MainArticle>();

                    List<StripePayment> stripePaymentsUser = new List<StripePayment>();
                    var stripeRepository = _stripeRepositoryFactory.New(connection);
                    List<StripePayment> stripePayments = stripeRepository.GetPaymentIUserdMainArticle(idUser);

                    foreach (var stripePayment in stripePayments)
                    {
                        if (mainArticles.Any(x => x.Id.Equals(stripePayment.MainArticleId)))
                        {
                            mainArticles.Where(x => x.Id == stripePayment.MainArticleId).ToList().ForEach(x => x.Articles.Add(stripePayment.SupArticle));
                        }
                        else
                        {
                            mainArticles.Add(stripePayment.MainArticle);
                            mainArticles.Where(x => x.Id == stripePayment.MainArticleId).ToList().ForEach(x => x.Articles.Add(stripePayment.SupArticle));
                        }
                    }

                    foreach (var stripePayment in stripePayments)
                    {

                        if (!stripePaymentsUser.Any(x => x.MainArticleId.Equals(stripePayment.MainArticleId)))
                        {
                            stripePaymentsUser.Add(stripePayment);
                        }
                    }

                    return stripePaymentsUser;
                }
            });

        public Task<MainArticle> GetArticleUser(Guid netUidArticle,long idUser) =>
            Task.Run(async () =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    MainArticle mainArticles = new MainArticle();
                    List<AuthorArticle> authorArticles = new List<AuthorArticle>();
                    List<AuthorArticle> filterAticle = new List<AuthorArticle>();
                    var stripeRepository = _stripeRepositoryFactory.New(connection);
                    var mainArticle = _mainRepositoryFactory.New(connection).GetArticle(netUidArticle);
                    var stripePayments = stripeRepository.GetPaymentMainArticle(idUser,mainArticle.Id);
                    foreach (var stripePayment in stripePayments)
                    {
                        if (mainArticles.Id.Equals(stripePayment.MainArticleId))
                        {
                            mainArticles.Articles.Add(stripePayment.SupArticle);
                        }
                        else
                        {
                            mainArticles = stripePayment.MainArticle;
                            mainArticles.Articles.Add(stripePayment.SupArticle);
                        }
                    }

                    return mainArticles;
                    //return mainArticles.First();

                }
            });

       
    }
} 
