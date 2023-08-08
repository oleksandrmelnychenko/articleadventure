using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domain.ArticleAdventure.Entities;

namespace service.ArticleAdventure.Services.Blog.Contracts
{
    public interface IArticleService
    {
        Task<long> AddArticle(AuthorArticle blog);
        Task<List<AuthorArticle>> GetAllArticles();
        Task Update(AuthorArticle blogs);
        Task Remove(Guid netUid);

        Task<AuthorArticle> GetArticle(Guid netUid);
    }
}
