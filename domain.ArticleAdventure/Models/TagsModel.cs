using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class TagsModel
    {
        public List<MainTag> MainTags { get; set; }
        public List<SupTag> SupTags { get; set; }

        public MainTag AddMainTag { get; set; }

        public SupTag AddSupTag { get; set; }
        public Guid ChangeMainTagsNetUid{ get; set; }
        public Guid ChangeSupTagsNetUid{ get; set; }

    }
}
