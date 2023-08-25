using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IStripeService
    {
        Task<CheckoutOrderResponse> BuyStripe(MainArticle mainArticle,string Email);
    }
}
