using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IStripeService
    {
        Task<ExecutionResult<CheckoutOrderResponse>> BuyStripeSupArticle(AuthorArticle mainArticle, string Email,string tokenUser);
        Task<CheckoutOrderResponse> BuyStripeMainArticle(MainArticle mainArticle,string Email,string tokenUser);
        Task<ExecutionResult<CheckoutOrderResponse>> BuyStripeCartArticle(List<MainArticle> mainArticle, string Email,string tokenUser);
        Task<ExecutionResult<List<StripePayment>>> CheckPaymentsHaveUser(string userEmail);
        Task CheckoutSuccess(string sessionId);
        Task CheckoutSuccessSup(string sessionId);
        Task CheckoutSuccessCart(string sessionId);
    }
}
