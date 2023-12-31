﻿using Dapper;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.IdentityEntities;
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
        public long AddCustomer(StripeCustomer stripeCustomer) =>
           _connection.Query<long>("INSERT INTO [stripeCustomers] " +
          "([Name], [Email], [UserId], [Updated] ) " +
          "VALUES " +
          "(@Name, @Email, @UserId, GETUTCDATE());" +
          "SELECT SCOPE_IDENTITY()", stripeCustomer
          ).Single();
        public long AddPayment(StripePayment payment)
           => _connection.Query<long>("INSERT INTO [stripePayments] " +
           "([MainArticleId], [SupArticleId], [UserId], [ReceiptEmail] ,[Description] ,[Currency] ,[PaymentStatus] ,[Amount] ,[Updated] ) " +
           "VALUES " +
           "(@MainArticleId, @SupArticleId, @UserId, @ReceiptEmail, @Description, @Currency, @PaymentStatus" +
           ", @Amount, GETUTCDATE());" +
           "SELECT SCOPE_IDENTITY()", payment
           ).Single();

        public List<StripePayment> GetPaymentIUserdMainArticle(long userId)
        {
            List<StripePayment> stripePayments = new List<StripePayment>();
            _connection.Query<StripePayment, MainArticle, AuthorArticle, StripePayment>("SELECT stripePayments.*, " +
            "mainArticle.*, " +
            "authorArticle.* " +
            "FROM [ArticleAdventure].[dbo].[stripePayments] as stripePayments " +
            "LEFT JOIN [ArticleAdventure].[dbo].[MainArticle] as mainArticle " +
            "ON stripePayments.MainArticleId = mainArticle.ID " +
            "LEFT JOIN [ArticleAdventure].[dbo].[AuthorArticle] as authorArticle " +
            "ON stripePayments.SupArticleId = authorArticle.ID " +
            "WHERE mainArticle.Deleted = 0 " +
            "AND authorArticle.Deleted = 0 " +
            "AND stripePayments.PaymentStatus = 'paid' " +
            "AND stripePayments.UserId = @UserId",
                (stripePayment, mainArticle, supArticle) =>
                {
                    if (stripePayments.Any(c => c.Id.Equals(stripePayment.Id)))
                    {
                        stripePayment = stripePayments.First(x => x.Id.Equals(stripePayment.Id));
                    }
                    else
                    {
                        stripePayments.Add(stripePayment);
                    }

                    if (stripePayments.Any(x => x.MainArticle.Id.Equals(mainArticle.Id)))
                    {
                        mainArticle = stripePayments.Select(x => x.MainArticle).First(x => x.Id.Equals(mainArticle.Id));
                    }
                    else
                    {
                        stripePayments.Where(x => x.MainArticleId == mainArticle.Id).ToList().ForEach(x => x.MainArticle = mainArticle);
                    }
                    if (stripePayments.Any(x => x.SupArticle.Id.Equals(supArticle.Id)))
                    {
                        supArticle = stripePayments.Select(x => x.SupArticle).First(x => x.Id.Equals(mainArticle.Id));
                    }
                    else
                    {
                        stripePayments.Where(x => x.SupArticleId == supArticle.Id).ToList().ForEach(x => x.SupArticle = supArticle);
                    }
                    return stripePayment;
                }, new { UserId = userId }).ToList();

            return stripePayments;
        }
        public List<StripePayment> GetPaymentEmailMainArticle(string receiptEmail)
        {
            List<StripePayment> stripePayments = new List<StripePayment>();
            _connection.Query<StripePayment, MainArticle, AuthorArticle, StripePayment>("SELECT stripePayments.*, " +
            "mainArticle.*, " +
            "authorArticle.* " +
            "FROM [ArticleAdventure].[dbo].[stripePayments] as stripePayments " +
            "LEFT JOIN [ArticleAdventure].[dbo].[MainArticle] as mainArticle " +
            "ON stripePayments.MainArticleId = mainArticle.ID " +
            "LEFT JOIN [ArticleAdventure].[dbo].[AuthorArticle] as authorArticle " +
            "ON stripePayments.SupArticleId = authorArticle.ID " +
            "WHERE mainArticle.Deleted = 0 " +
            "AND authorArticle.Deleted = 0 " +
            "AND stripePayments.PaymentStatus = 'paid' " +
            "AND stripePayments.ReceiptEmail = @ReceiptEmail",
                (stripePayment, mainArticle, supArticle) =>
                {
                    if (stripePayments.Any(c => c.Id.Equals(stripePayment.Id)))
                    {
                        stripePayment = stripePayments.First(x => x.Id.Equals(stripePayment.Id));
                    }
                    else
                    {
                        stripePayments.Add(stripePayment);
                    }

                    if (stripePayments.Any(x => x.MainArticle.Id.Equals(mainArticle.Id)))
                    {
                        mainArticle = stripePayments.Select(x => x.MainArticle).First(x => x.Id.Equals(mainArticle.Id));
                    }
                    else
                    {
                        stripePayments.Where(x => x.MainArticleId == mainArticle.Id).ToList().ForEach(x => x.MainArticle = mainArticle);
                    }
                    if (stripePayments.Any(x => x.SupArticle.Id.Equals(supArticle.Id)))
                    {
                        supArticle = stripePayments.Select(x => x.SupArticle).First(x => x.Id.Equals(mainArticle.Id));
                    }
                    else
                    {
                        stripePayments.Where(x => x.SupArticleId == supArticle.Id).ToList().ForEach(x => x.SupArticle = supArticle);
                    }
                    return stripePayment;
                }, new { ReceiptEmail = receiptEmail }).ToList();

            return stripePayments;
        }
        public List<StripePayment> GetPaymentMainArticle(long userId, long mainArticleId)
        {
            List<StripePayment> stripePayments = new List<StripePayment>();
            _connection.Query<StripePayment, MainArticle, AuthorArticle, StripePayment>("SELECT stripePayments.*, " +
            "mainArticle.*, " +
            "authorArticle.* " +
            "FROM [ArticleAdventure].[dbo].[stripePayments] as stripePayments " +
            "LEFT JOIN [ArticleAdventure].[dbo].[MainArticle] as mainArticle " +
            "ON stripePayments.MainArticleId = mainArticle.ID " +
            "LEFT JOIN [ArticleAdventure].[dbo].[AuthorArticle] as authorArticle " +
            "ON stripePayments.SupArticleId = authorArticle.ID " +
            "WHERE mainArticle.Deleted = 0 " +
            "AND authorArticle.Deleted = 0 " +
            "AND stripePayments.PaymentStatus = 'paid' " +
            "AND stripePayments.UserId = @UserId " +
            "AND stripePayments.MainArticleId = @MainArticleId",
                (stripePayment, mainArticle, supArticle) =>
                {
                    if (stripePayments.Any(c => c.Id.Equals(stripePayment.Id)))
                    {
                        stripePayment = stripePayments.First(x => x.Id.Equals(stripePayment.Id));
                    }
                    else
                    {
                        stripePayments.Add(stripePayment);
                    }

                    if (stripePayments.Any(x => x.MainArticle.Id.Equals(mainArticle.Id)))
                    {
                        mainArticle = stripePayments.Select(x => x.MainArticle).First(x => x.Id.Equals(mainArticle.Id));
                    }
                    else
                    {
                        stripePayments.Where(x => x.MainArticleId == mainArticle.Id).ToList().ForEach(x => x.MainArticle = mainArticle);
                    }
                    if (stripePayments.Any(x => x.SupArticle.Id.Equals(supArticle.Id)))
                    {
                        supArticle = stripePayments.Select(x => x.SupArticle).First(x => x.Id.Equals(mainArticle.Id));
                    }
                    else
                    {
                        stripePayments.Where(x => x.SupArticleId == supArticle.Id).ToList().ForEach(x => x.SupArticle = supArticle);
                    }
                    return stripePayment;
                }, new
                {
                    UserId = userId,
                    MainArticleId = mainArticleId,
                }).ToList();

            return stripePayments;
        }
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

        public void SetStatusPayment(Guid netUidPayment)
            => _connection.Execute("Update [stripePayments] " +
                "SET [PaymentStatus] = @PaymentStatus,[Updated] = GETUTCDATE() " +
                $"WHERE [stripePayments].[NetUid] = @NetUid ",
                new
                {
                    NetUid = netUidPayment,
                    PaymentStatus = "paid"
                });



        public StripeCustomer GetCustomer(long userId) =>
            _connection.Query<StripeCustomer>("SELECT * FROM [stripeCustomers] AS Payment " +
                "WHERE Payment.UserId = @UserId",
                new
                {
                    UserId = userId,
                }
           ).SingleOrDefault();

        public List<StripeCustomer> GetallCustomer() =>
            _connection.Query<StripeCustomer>("SELECT * FROM [stripeCustomers] AS Customer " +
                "WHERE Customer.Deleted = 0 " +
                "AND Customer.PaymentStatus = 'paid' "
           ).ToList();

        public List<StripePayment> GetAllPayment() =>
            _connection.Query<StripePayment>("SELECT * FROM [stripePayments] AS Payment " +
                 "WHERE Payment.Deleted = 0 " +
                 "AND Payment.PaymentStatus = 'paid' "
           ).ToList();
        public List<StripePayment> GetDaysPayment(int days) =>
            _connection.Query<StripePayment>("SELECT * FROM [stripePayments] AS Payment " +
            "WHERE Payment.Deleted = 0 " +
            "AND Payment.PaymentStatus = 'paid' " +
            "AND Payment.Updated >= DATEADD(day,@Days, GETDATE()) ",
                new
                {
                    Days = -days
                }
           ).ToList();
        public List<StripeCustomer> GetDaysCustomer(int days) =>
            _connection.Query<StripeCustomer>("SELECT * FROM [stripeCustomers] AS Customer " +
            "WHERE Customer.Deleted = 0 " +
            "AND Customer.Updated >= DATEADD(day,@Days, GETDATE()) ",
                new
                {
                    Days = -days
                }
           ).ToList();
    }
}
