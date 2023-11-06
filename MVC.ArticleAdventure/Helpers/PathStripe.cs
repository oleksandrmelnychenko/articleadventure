namespace MVC.ArticleAdventure.Helpers
{
    public class PathStripe
    {
        public const string BUY_NOW_STRIPE_CART_ARTICLE = "api/v1/stripe/checkout/buynow/cart";
        public const string BUY_NOW_STRIPE_SUP_ARTICLE = "api/v1/stripe/checkout/buynow/sup";
        public const string BUY_NOW_STRIPE_MAIN_ARTICLE = "api/v1/stripe/checkout/buynow";
        public const string BUY_CART_STRIPE = "api/v1/stripe/checkout/buycart";
        public const string CHECK_PAYMENTS_HAVE_USER = "api/v1/stripe/check/payments/user";
        public const string GET_ALL_STRIPE_PAYMENTS = "api/v1/stripe/get/all/payments";
        public const string GET_ALL_STRIPE_CUSTOMERS = "api/v1/stripe/get/all/customers";
        public const string GET_ALL_STRIPE_STATISTICS = "api/v1/stripe/get/all/statistick";


        public const string CHECK_SUCCESS = "api/v1/stripe/checkout/success";
        public const string CHECK_SUCCESS_SUP = "api/v1/stripe/checkout/success/sup";
        public const string CHECK_SUCCESS_CART = "api/v1/stripe/checkout/success/cart";
    }
}
