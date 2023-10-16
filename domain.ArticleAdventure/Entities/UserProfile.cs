using domain.ArticleAdventure.Helpers;
using Microsoft.Extensions.Configuration.UserSecrets;
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
        public string GetLinkPictureUser() => Path.Combine(ArticleAdventureFolderManager.GetServerUrl(), LinkPictureUser);

        //public string GetLinkPictureUser => PathHepler.Url + LinkInstagram;
        public string LinkPictureUser { get; set; }
        public string LinkInstagram { get; set; }
        public string LinkTwitter { get; set; }
        public string LinkFacebook { get; set; }
        public string LinkTelegram { get; set; }
        public bool GrantAdministrativePermissions { get; set; }
        public List<MainArticle> mainArticles { get; set; }
        public string Role { get;set; }
    }
}