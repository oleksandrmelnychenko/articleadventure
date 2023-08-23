using domain.ArticleAdventure.Entities;

namespace domain.ArticleAdventure.Models
{
    public class NewArticleModel
    {
        public List<MainTag> MainTags { get; set; }

        public List<SupTag> SelectSupTags { get; set; } = new List<SupTag>();

        public List<AuthorArticle> authorArticles { get; set; } = new List<AuthorArticle>();

        public AuthorArticle authorArticle { get; set; }
        public Guid NetUidTags { get; set; }
    }
}
