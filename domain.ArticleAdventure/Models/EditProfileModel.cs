using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class EditProfileModel
    {
        public UserProfile UserProfile { get; set; }
        public IFormFile PhotoMainArticle { get; set; }
    }
}
