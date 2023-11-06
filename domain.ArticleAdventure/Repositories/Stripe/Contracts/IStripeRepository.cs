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
        public List<StripePayment> GetAllPayment();
        public List<StripeCustomer> GetDaysCustomer(int days);
        public List<StripePayment> GetDaysPayment(int days);
        public long AddCustomer(StripeCustomer stripeCustomer);
        public StripeCustomer GetCustomer(long UserId);
        public List<StripeCustomer> GetallCustomer();

        public void SetStatusPayment(Guid netUidPayment);
        public List<StripePayment> GetPaymentIUserdMainArticle(long UserId);
        List<StripePayment> GetPaymentMainArticle(long userId, long mainArticleId);
    }
}
