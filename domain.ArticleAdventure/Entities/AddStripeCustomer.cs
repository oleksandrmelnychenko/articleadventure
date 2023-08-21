using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    public class AddStripeCustomer
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public AddStripeCard CreditCard { get; set; } 
    }
}
