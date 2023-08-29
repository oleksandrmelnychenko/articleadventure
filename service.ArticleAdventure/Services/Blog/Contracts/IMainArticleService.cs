﻿using domain.ArticleAdventure.Entities;
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
        Task<List<MainArticle>> GetAllArticles();
        Task Update(MainArticle blogs);
        Task Remove(Guid netUid);
        Task<MainArticle> GetArticle(Guid netUid);
    }
}
