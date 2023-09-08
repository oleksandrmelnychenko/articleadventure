using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IStripeService
    {
        Task<ExecutionResult<CheckoutOrderResponse>> BuyStripeSupArticle(AuthorArticle mainArticle, string Email);
        Task<CheckoutOrderResponse> BuyStripeMainArticle(MainArticle mainArticle,string Email);
        Task<ExecutionResult<List<StripePayment>>> CheckPaymentsHaveUser(string userEmail);
        Task CheckoutSuccess(string sessionId);
        Task CheckoutSuccessSup(string sessionId);
    }
}
