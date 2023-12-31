﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities 
{
    public class SupTag:EntityBase
    {
        [Required(ErrorMessage = "Field {0} is required")]
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public string Color { get; set; }
        public long IdMainTag { get; set; }
        public MainTag MainTag { get; set; }
        public ICollection<MainArticleTags> MainArticleTags { get; set; }
    }
}
