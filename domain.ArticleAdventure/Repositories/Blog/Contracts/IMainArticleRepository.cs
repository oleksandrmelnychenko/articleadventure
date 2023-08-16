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

        void Remove(Guid netUid);

        void Update(MainArticle blog);

        MainArticle GetArticle(Guid netUid);
    }
}
