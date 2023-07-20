using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace data.ArticleAdventure.Views.New
{
    public class NewModel : PageModel
    {
        public Blogs Blogs { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }

        public void OnGet()
        {

        }
        public void OnPostSubmit()
        {
           
        }
        //public void OnPost() 
        //{
        //    Title = "22";
        //    Description = "22";
        //    Body = "22";
        //}
        public void OnPost()
        {
            var name = Request.Form["Name"];
            var email = Request.Form["Email"];
            ViewData["confirmation"] = $"{name}, information will be sent to {email}";
        }
    }
  
}
