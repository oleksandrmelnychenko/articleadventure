using System.ComponentModel.DataAnnotations;

namespace domain.ArticleAdventure.Entities
{
    public class MainTag:EntityBase
    {
        [Required(ErrorMessage = "Field {0} is required")]
        public string Name { get; set; }
        public string Color { get; set; }
        public bool IsSelected { get; set; }
        public ICollection<SupTag> SubTags { get; set; }

        public MainTag()
        {
            SubTags = new List<SupTag>();
        }
    }
}
