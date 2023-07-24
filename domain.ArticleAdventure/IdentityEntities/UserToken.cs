using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.IdentityEntities
{
    public sealed class UserToken
    {
        public long Id { get; set; }

        public string Token { get; set; }

        public string UserId { get; set; }
    }
}
