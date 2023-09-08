namespace MVC.ArticleAdventure.Helpers
{
    public class PathStripe
    {
        public const string BUY_NOW_STRIPE_SUP_ARTICLE = "api/v1/stripe/checkout/buynow/sup";
        public const string BUY_NOW_STRIPE_MAIN_ARTICLE = "api/v1/stripe/checkout/buynow";
        public const string BUY_CART_STRIPE = "api/v1/stripe/checkout/buycart";
        public const string CHECK_PAYMENTS_HAVE_USER = "api/v1/stripe/check/payments/user";
        public const string CHECK_SUCCESS = "api/v1/stripe/checkout/success";
        public const string CHECK_SUCCESS_SUP = "api/v1/stripe/checkout/success/sup";
    }
}
