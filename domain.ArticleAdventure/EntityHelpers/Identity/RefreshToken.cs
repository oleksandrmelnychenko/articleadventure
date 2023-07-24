using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.EntityHelpers.Identity
{
    public sealed class RefreshToken
    {
        public string UserId { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}
