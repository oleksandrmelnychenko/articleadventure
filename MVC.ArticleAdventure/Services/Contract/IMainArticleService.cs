using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
     public interface IMainArticleService
    {
        public Task<MainArticle> GetArticle(Guid netUidArticle);
        public Task<MainArticle> GetArticle(long id);
        public Task<List<MainArticle>> GetAllArticles();
        public Task AddArticle(MainArticle article,IFormFile photoMainArticle);
        public Task Update(MainArticle article, IFormFile photoMainArticle);
        public Task Remove(Guid netUidArticle);
        public Task<List<MainArticle>> GetAllArticles(long idUser);
    }
}
