using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.WebApi.RoutingConfiguration.Maps
{
    public static class StripeSegments
    {
        public const string CHECKOUT_BUY_NOW_SUP_ARTICLE = "checkout/buynow/sup";
        public const string CHECKOUT_BUY_NOW_MAIN_ARTICLE = "checkout/buynow";
        public const string CHECKOUT_BUY_CART = "checkout/buycart";

        public const string CHECKOUT_SUCCESS_MAIN_ARTICLE = "checkout/success";
        public const string CHECKOUT_SUCCESS_SUP_ARTICLE = "checkout/success/sup";
        public const string CHECKOUT_FAILED = "checkout/failed";

        public const string CHECK_PAYMENTS_HAVE_USER = "check/payments/user";

        public const string ADD_STRIPE_CUSTOMER = "customer/add";
        public const string ADD_STRIPE_PAYMENT = "payment/add";
    }
}
