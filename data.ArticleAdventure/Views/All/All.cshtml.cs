using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace data.ArticleAdventure.Views.All
{
    public class AllModel : PageModel
    {
        public List<Blogs> Blogs { get; set; }
        public void OnGet()
        {
        }
    }
}
