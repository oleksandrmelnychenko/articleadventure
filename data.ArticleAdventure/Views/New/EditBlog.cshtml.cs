using data.ArticleAdventure.Models;
using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace data.ArticleAdventure.Views.New
{
    public class EditBlogModel : PageModel
    {
        public Blogs Blogs { get; set; }
    }
}
