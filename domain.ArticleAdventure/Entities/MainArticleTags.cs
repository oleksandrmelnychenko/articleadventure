namespace domain.ArticleAdventure.Entities
{
    public class MainArticleTags: EntityBase
    {
        public long MainArticleId { get; set; }
        public MainArticle MainArticle { get; set; }
        public long SupTagId { get; set; }
        public SupTag SupTag { get; set; }
    }
}
