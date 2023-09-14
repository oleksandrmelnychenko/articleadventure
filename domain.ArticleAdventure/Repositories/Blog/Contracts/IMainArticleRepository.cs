using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Blog.Contracts
{
    public interface IMainArticleRepository
    {
        long AddMainArticle(MainArticle blog);

        List<MainArticle> GetAllArticles();
        List<MainArticle> GetAllArticlesFilterSupTag(List<MainArticleTags> supTags);

        void RemoveMainArticle(Guid netUid);

        void UpdateMainArticle(MainArticle blog);
        AuthorArticle GetSupArticle(Guid netUid);

        MainArticle GetArticle(Guid netUid);
        MainArticle GetArticle(long id);
    }
}
