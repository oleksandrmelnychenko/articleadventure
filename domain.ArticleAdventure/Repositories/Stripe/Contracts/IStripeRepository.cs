using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Stripe.Contracts
{
    public interface IStripeRepository
    {
        public long AddPayment(StripePayment payment);
        public List<StripePayment> GetPaymentEmailMainArticle(string receiptEmail);
        public List<StripePayment> CheckPaymentMainArticle(string receiptEmail, long mainArticleid, long UserId);
        public StripePayment GetPayment(long MainArticleId, long SupArticleId, long UserId);
        public void SetStatusPayment(Guid netUidPayment, string paymentStatus);
        public List<StripePayment> GetPaymentIUserdMainArticle(long UserId);
        List<StripePayment> GetPaymentMainArticle(long userId, long mainArticleId);
    }
}
