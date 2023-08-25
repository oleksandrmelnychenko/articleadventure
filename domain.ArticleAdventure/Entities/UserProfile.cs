using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    public sealed class UserProfile : EntityBase
    {
        public string UserName { get; set; }
        public string SurName { get; set; }
        public string InformationAccount { get; set; }
        public string Email { get; set; }
        public string LinkPictureUser { get; set; }
        public bool GrantAdministrativePermissions { get; set; }
    }
}