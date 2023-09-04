using Dapper;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Stripe.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Stripe
{
    public class StripeRepository : IStripeRepository
    {
        private readonly IDbConnection _connection;
        public StripeRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public long AddPayment(StripePayment payment)
           => _connection.Query<long>("INSERT INTO [stripePayments] " +
           "([MainArticleId], [SupArticleId], [UserId], [ReceiptEmail] ,[Description] ,[Currency] ,[PaymentStatus] ,[Amount] ,[Updated] ) " +
           "VALUES " +
           "(@MainArticleId, @SupArticleId, @UserId, @ReceiptEmail, @Description, @Currency, @PaymentStatus" +
           ", @Amount, GETUTCDATE());" +
           "SELECT SCOPE_IDENTITY()", payment
           ).Single();

        public List<StripePayment> GetPaymentIUserdMainArticle(long userId)
            => _connection.Query<StripePayment>("SELECT * FROM [stripePayments] AS Payment " +
                "WHERE Payment.UserId = @UserId " +
                "AND PaymentStatus ='paid'",
                new
                {
                    UserId = userId,
                }).ToList();
        public List<StripePayment> GetPaymentEmailMainArticle(string receiptEmail)
            => _connection.Query<StripePayment>("SELECT * FROM [stripePayments] AS Payment " +
                "WHERE Payment.ReceiptEmail = @ReceiptEmail " +
                "AND PaymentStatus ='paid'",
                new
                {
                    ReceiptEmail = receiptEmail,
                }).ToList();
        public List<StripePayment> CheckPaymentMainArticle(string receiptEmail, long mainArticleid, long userId)
            => _connection.Query<StripePayment>("SELECT * FROM [stripePayments] AS Payment " +
            "WHERE Payment.ReceiptEmail = @ReceiptEmail" +
            "AND Payment.MainArticleId = @MainArticleId " +
            "AND Payment.UserId = @UserId " +
                "AND PaymentStatus = @PaymentStatus ",
                new
                {
                    ReceiptEmail = receiptEmail,
                    MainArticleId = mainArticleid,
                    UserId = userId,
                    PaymentStatus = "unpaid"
                }).ToList();
        public StripePayment GetPayment(long mainArticleId, long supArticleId, long userId)
            => _connection.Query<StripePayment>("SELECT * FROM [stripePayments] AS Payment " +
            "WHERE Payment.MainArticleId = @MainArticleId " +
            "AND Payment.SupArticleId = @SupArticleId " +
            "AND Payment.UserId = @UserId " +
            "AND Payment.PaymentStatus = @PaymentStatus ",
                new
                {
                    MainArticleId = mainArticleId,
                    SupArticleId = supArticleId,
                    UserId = userId,
                    PaymentStatus = "unpaid"
                }
           ).SingleOrDefault();

        public void SetStatusPayment(Guid netUidPayment, string paymentStatus)
            => _connection.Execute("Update [stripePayments] " +
                "SET [PaymentStatus] = @PaymentStatus,[Updated] = GETUTCDATE() " +
                $"WHERE [stripePayments].[NetUid] = @NetUid ",
                new
                {
                    NetUid = netUidPayment,
                    PaymentStatus = paymentStatus
                });

       
    }
}
