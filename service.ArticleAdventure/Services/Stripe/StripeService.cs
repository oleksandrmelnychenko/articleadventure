using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Repositories.Identity.Contracts;
using domain.ArticleAdventure.Repositories.Stripe.Contracts;
using service.ArticleAdventure.Services.Stripe.Contracts;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services.Stripe
{
    public class StripeService : IStripeService
    {
        private readonly ChargeService _chargeService;
        private readonly CustomerService _customerService;
        private readonly TokenService _tokenService;
        private readonly IIdentityRepositoriesFactory _identityRepositoriesFactory;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IStripeRepositoryFactory _stripeRepositoryFactory;

        public StripeService(
            ChargeService chargeService,
            CustomerService customerService,
            TokenService tokenService,
            IIdentityRepositoriesFactory identityRepositoriesFactory,
            IDbConnectionFactory connectionFactory, IStripeRepositoryFactory stripeRepositoryFactory)
        {
            _chargeService = chargeService;
            _customerService = customerService;
            _tokenService = tokenService;
            _identityRepositoriesFactory = identityRepositoriesFactory;
            _connectionFactory = connectionFactory;
            _stripeRepositoryFactory = stripeRepositoryFactory;
        }

        public async Task<List<StripePayment>> CheckPaymentsHaveUser(string userEmail)
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                IUserProfileRepository userProfileRepository =
                           _identityRepositoriesFactory.NewUserProfileRepository(connection);
                UserProfile userProfile = userProfileRepository.Get(userEmail);
                var stripeRepositoryFactory = _stripeRepositoryFactory.New(connection);
                var Payments = stripeRepositoryFactory.GetPaymentEmailMainArticle(userEmail);
                return Payments;
            }
        }

        public async Task<CheckoutOrderResponse> CheckOutBuyNow(MainArticle mainArticle, string thisApiUrl, string userEmail)
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                IUserProfileRepository userProfileRepository =
                           _identityRepositoriesFactory.NewUserProfileRepository(connection);

                UserProfile userProfile = userProfileRepository.Get(userEmail);
                var stripeRepository = _stripeRepositoryFactory.New(connection);

                List<StripePayment> stripePayments = new List<StripePayment>();

                List<StripePayment> checkStripePayments = new List<StripePayment>();


                foreach (var supArticle in mainArticle.Articles)
                {
                    var Payments = stripeRepository.GetPayment(mainArticle.Id, supArticle.Id, userProfile.Id);
                    if (Payments != null)
                    {
                        checkStripePayments.Add(Payments);
                    }

                }
                StripeCustomer stripeCustomer = new StripeCustomer
                {
                    UserId = userProfile.Id,
                    Name = userProfile.UserName + userProfile?.SurName,
                    Email = userProfile.Email,
                };

                var metaData = new Dictionary<string, string>()
                {

                   { "IDMainArticle", mainArticle.Id.ToString() },
                   { "NetUidUser", userProfile.NetUid.ToString() }
                };



                for (int i = 0; i < mainArticle.Articles.Count; i++)
                {
                    metaData.Add($"IDSupArticle{i + 1}", mainArticle.Articles[i].Id.ToString());
                }

                var options = new SessionCreateOptions
                {
                    Metadata = metaData,
                    CustomerEmail = userEmail,
                    // Stripe calls the URLs below when certain checkout events happen such as success and failure.
                    SuccessUrl = $"{ArticleAdventureFolderManager.GetClientPath()}/Stripe/SuccessBuy?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
                    CancelUrl = $"{ArticleAdventureFolderManager.GetServerPath()}/api/v1/stripe/checkout/failed",  // Checkout cancelled.
                    PaymentMethodTypes = new List<string> // Only card available in test mode?
                {
                    "card"
                },
                    //Metadata = metaData,
                    LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(mainArticle.Price * 100),
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = mainArticle.Title,
                            Description = mainArticle.Description
                            //Metadata = metaData
                            //Images = new List<string> { mainArticle.ImageUrl }
                        },


                    },
                    Quantity = 1,
                },
            },
                    Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
                };



                var service = new SessionService();
                var session = await service.CreateAsync(options);

                var pubKey = "pk_test_51NdTsYLwAUt6I3BOPOMdxTKOoBPd9UErIyY7J7NY4XwOTDHHfmeFGbcQK18m4KJ6x1g9UZ5TizySV8TxnhamzQbS00cbtKuMRf";

                var checkoutOrderResponse = new CheckoutOrderResponse()
                {
                    SessionId = session.Id,
                    PubKey = pubKey
                };
                foreach (var article in mainArticle.Articles)
                {
                    StripePayment stripePayment = new StripePayment
                    {
                        MainArticleId = mainArticle.Id,
                        SupArticleId = article.Id,
                        PaymentStatus = session.PaymentStatus,
                        ReceiptEmail = userProfile.Email,
                        UserId = userProfile.Id,
                        Description = article.Description,
                        Currency = "USD",
                        Amount = article.Price,
                    };
                    stripePayments.Add(stripePayment);
                }

                if (checkStripePayments.Count() == 0)
                {
                    foreach (var payment in stripePayments)
                    {
                        stripeRepository.AddPayment(payment);
                    }
                }

                return checkoutOrderResponse;
            }

        }

        public async Task CheckoutSuccess(string sessionId)
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                IUserProfileRepository userProfileRepository =
                          _identityRepositoriesFactory.NewUserProfileRepository(connection);

                List<StripePayment> stripePayments = new List<StripePayment>();

                var sessionService = new SessionService();
                Session session = sessionService.Get(sessionId);
                var paymentService = new PaymentIntentService();
                PaymentIntent x = paymentService.Get(session.PaymentIntentId);

                var amountTotal = session.AmountTotal.Value;
                var Name = session.CustomerDetails.Name;
                var customerEmail = session.CustomerDetails.Email;

                Dictionary<string, string> metaData = session.Metadata;
                var keys = metaData.Keys.ToList();

                var MainArticleId = metaData["IDMainArticle"];
                var NetUidUser = metaData["NetUidUser"];
                metaData.Remove("IDMainArticle");
                metaData.Remove("NetUidUser");
                var valuesSupArticles = metaData.Values.ToList();
                var stripeRepositoryFactory = _stripeRepositoryFactory.New(connection);

                UserProfile userProfile = userProfileRepository.Get(Guid.Parse(NetUidUser));

                foreach (var idSupArticle in metaData.Values.ToList())
                {
                    StripePayment s = stripeRepositoryFactory.GetPayment(long.Parse(MainArticleId), long.Parse(idSupArticle), userProfile.Id);
                    stripePayments.Add(s);
                }

                foreach (var payment in stripePayments)
                {
                    stripeRepositoryFactory.SetStatusPayment(payment.NetUid, session.PaymentStatus);
                }

            }
        }


    }
}
