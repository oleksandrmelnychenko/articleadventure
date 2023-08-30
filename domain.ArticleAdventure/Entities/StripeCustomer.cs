using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    public class StripeCustomer:EntityBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}
