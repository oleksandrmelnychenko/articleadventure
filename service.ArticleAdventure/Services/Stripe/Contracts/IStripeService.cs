using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services.Stripe.Contracts
{
    public interface IStripeService
    {

        Task<List<StripePayment>> CheckPaymentsHaveUser(string userMail);
        Task<List<StripePayment>> GetAllPayment();
        Task<List<StripeCustomer>> GetAllCustomer();
        Task<PaymentStatistics> GetStatisticsDays(int days);
        Task<CheckoutOrderResponse> CheckOutBuyNowMainArticle(MainArticle mainArticle,string userEmail);
        Task<CheckoutOrderResponse> CheckOutBuyCartMainArticle(List<MainArticle> mainArticle,string userEmail);
        Task<CheckoutOrderResponse> CheckOutBuyNowSupArticle(AuthorArticle mainArticle,string userEmail);
        Task CheckoutSuccessMainArticle(string sessionId);
        Task CheckoutSuccessSupArticle(string sessionId);
        Task CheckoutSuccessCartArticle(string sessionId);
    }
}
