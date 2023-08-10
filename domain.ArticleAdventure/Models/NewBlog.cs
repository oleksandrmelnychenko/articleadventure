using domain.ArticleAdventure.Entities;

namespace domain.ArticleAdventure.Models
{
    public class NewBlogModel
    {
        public List<MainTag> MainTags { get; set; }

        public List<SupTag> SelectSupTags { get; set; } = new List<SupTag>();

        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public Guid NetUidTags { get; set; }
    }
}
