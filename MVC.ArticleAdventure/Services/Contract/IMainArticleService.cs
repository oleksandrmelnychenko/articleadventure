using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
     public interface IMainArticleService
    {
        public Task<ExecutionResult<MainArticle>> GetArticleUser(Guid netUidArticle, long idUser);
        public Task<ExecutionResult<List<MainArticle>>> GetAllArticlesUser(long idUser);

        public Task<ExecutionResult<AuthorArticle>> GeSupArticle(Guid netUidArticle);
        public Task<MainArticle> GetArticle(Guid netUidArticle);
        public Task<MainArticle> GetArticle(long id);
        public Task<List<MainArticle>> GetAllArticles();
        public Task<ExecutionResult<long>> AddArticle(MainArticle article,IFormFile photoMainArticle);
        public Task<ExecutionResult<long>> Update(MainArticle article, IFormFile photoMainArticle);
        public Task Remove(Guid netUidArticle);
    }
}
