using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace data.ArticleAdventure.Views.New
{
    public class NewModel : PageModel
    {
        public string PersonalDetails { get; set; }

        public void OnGet()
        {
        }
        public void OnPostSubmit(string personalDetails)
        {
            this.PersonalDetails = personalDetails;
        }
    }
}
