using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace data.ArticleAdventure.Views.New
{
    public class NewModel : PageModel
    {
        public Blogs Blogs { get; set; }
        public long Id { get; set; }

        public void OnGet()
        {

        }
    }
  
}
