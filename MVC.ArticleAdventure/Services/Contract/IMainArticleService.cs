using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
     public interface IMainArticleService
    {
        public Task<ExecutionResult<long>> Update(MainArticle article, IFormFile photoMainArticle, string tokenAdmin);
        public Task<ExecutionResult<long>> Remove(Guid netUidArticle, string tokenAdmin);
        public Task<ExecutionResult<long>> AddArticle(MainArticle article, IFormFile photoMainArticle, string tokenAdmin);
        public Task<ExecutionResult<MainArticle>> GetArticleUser(Guid netUidArticle, long idUser,string tokenUser);
        public Task<ExecutionResult<List<MainArticle>>> GetAllArticlesUser(long idUser,string tokenUser);
        public Task<ExecutionResult<List<StripePayment>>> GetAllStripePaymentsUser(long idUser);//token user під питанням
        public Task<ExecutionResult<AuthorArticle>> GeSupArticle(Guid netUidArticle);//під питанням 
        public Task<MainArticle> GetArticle(Guid netUidArticle);
        public Task<MainArticle> GetArticle(long id);
        public Task<List<MainArticle>> GetAllArticles();
        public Task<ExecutionResult<List<MainArticle>>> GetAllFilterDateTimeArticles();
        public Task<ExecutionResult<List<MainArticle>>> GetAllArticlesFilterSupTags(List<MainArticleTags> mainArticleTags);
       
    }
}
