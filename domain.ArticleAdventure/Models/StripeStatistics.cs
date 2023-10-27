using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class StripeStatistics
    {
        public List<StripePayment> stripePayments { get; set; }
        public List<StripeCustomer> stripeCustomer { get; set; }
    }
}
