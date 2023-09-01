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
        Task<CheckoutOrderResponse> CheckOutBuyNow(MainArticle mainArticle, string thisApiUrl,string userEmail);
        Task CheckoutSuccess(string sessionId);
    }
}
