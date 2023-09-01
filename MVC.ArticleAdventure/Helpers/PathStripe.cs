namespace MVC.ArticleAdventure.Helpers
{
    public class PathStripe
    {
        public const string BUY_NOW_STRIPE = "api/v1/stripe/checkout/buynow";
        public const string BUY_CART_STRIPE = "api/v1/stripe/checkout/buycart";
        public const string CHECK_PAYMENTS_HAVE_USER = "api/v1/stripe/check/payments/user";
        public const string CHECK_SUCCESS = "api/v1/stripe/checkout/success";
    }
}
