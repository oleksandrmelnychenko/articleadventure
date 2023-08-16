using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
     public interface IMainArticleService
    {
        public Task<MainArticle> GetArticle(Guid netUidArticle);
        public Task<List<MainArticle>> GetAllArticles();
        public Task AddArticle(MainArticle article);
        public Task Update(MainArticle article);
        public Task Remove(Guid netUidArticle);
    }
}
