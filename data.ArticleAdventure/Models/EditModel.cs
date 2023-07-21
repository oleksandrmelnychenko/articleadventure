using domain.ArticleAdventure.Entities;

namespace data.ArticleAdventure.Models
{
    public class EditModel
    {
        public Guid NetUid { get; set; }
        public Blogs? Blogs { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
    }
}
