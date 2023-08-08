using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    public class SupTag:EntityBase
    {
        public long IdMainTag { get; set; }
        public MainTag MainTag { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public string Color { get; set; }
    }
}
