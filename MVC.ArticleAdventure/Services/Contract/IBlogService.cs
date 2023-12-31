﻿using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IArticleService
    {
        public Task<AuthorArticle> GetArticle(Guid netUidArticle);
        public Task<List<AuthorArticle>> GetAllArticles();
        public Task AddArticle(AuthorArticle Article);
        public Task Update(AuthorArticle Article,string tokenAdmin);//token Admin
        public Task Remove(Guid netUidArticle, string tokenAdmin);//Token Admin

    }
}
