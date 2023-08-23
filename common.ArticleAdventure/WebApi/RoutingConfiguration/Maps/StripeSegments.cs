using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.WebApi.RoutingConfiguration.Maps
{
    public static class StripeSegments
    {
        public const string CHECKOUT = "checkout";
        public const string CHECKOUT_SUCCESS = "checkout/success";
        public const string ADD_STRIPE_CUSTOMER = "customer/add";
        public const string ADD_STRIPE_PAYMENT = "payment/add";
    }
}
