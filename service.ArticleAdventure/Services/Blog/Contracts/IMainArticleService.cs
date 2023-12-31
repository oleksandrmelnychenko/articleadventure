﻿using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.EntityHelpers.Filter;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services.Blog.Contracts
{
    public interface IMainArticleService
    {
        Task<long> AddArticle(MainArticle blog,IFormFile PhotoMainArticle);
        Task<List<MainArticle>> GetAllArticles(int page = 1, int count = 25);
        Task<List<MainArticle>> GetAll(MainArticleFilter filter,int page = 1, int count = 25);
        Task<List<MainArticle>> GetAllFilterDateTimeArticles();
        Task<List<MainArticle>> GetAllArticlesFilterSupTags(List<MainArticleTags> mainTags, int page = 1, int count = 25);
        Task<long> Update(MainArticle blogs, IFormFile filePhotoMainArticle);
        Task Remove(Guid netUid);
        Task<AuthorArticle> GetSupArticle(Guid netUid);
        Task<MainArticle> GetArticle(Guid netUid);
        Task<MainArticle> GetArticle(long id);
        Task<List<MainArticle>> GetAllArticlesUser(long idUser);
        Task<List<StripePayment>> GetAllPaymentArticleUser(long idUser);
        Task<MainArticle> GetArticleUser(Guid netUidArticle, long idUser);

    }
}
