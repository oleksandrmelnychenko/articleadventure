using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
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
        private readonly IMainArticleRepositoryFactory _mainArticleRepositoryFactory;

        public StripeService(
            ChargeService chargeService,
            CustomerService customerService,
            TokenService tokenService,
            IIdentityRepositoriesFactory identityRepositoriesFactory,
            IDbConnectionFactory connectionFactory, IStripeRepositoryFactory stripeRepositoryFactory, IMainArticleRepositoryFactory mainArticleRepositoryFactory)
        {
            _chargeService = chargeService;
            _customerService = customerService;
            _tokenService = tokenService;
            _identityRepositoriesFactory = identityRepositoriesFactory;
            _connectionFactory = connectionFactory;
            _stripeRepositoryFactory = stripeRepositoryFactory;
            _mainArticleRepositoryFactory = mainArticleRepositoryFactory;
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

        public async Task<CheckoutOrderResponse> CheckOutBuyNowMainArticle(MainArticle mainArticle, string userEmail)
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
                SessionCreateOptions options = SessionCreate(mainArticle.Price, mainArticle.Title, mainArticle.Description, userEmail, metaData, "SuccessBuy");

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

        private static SessionCreateOptions SessionCreate(double price, string title, string description, string userEmail, Dictionary<string, string> metaData, string UrlSuccess)
        {
            var options = new SessionCreateOptions
            {
                Metadata = metaData,
                CustomerEmail = userEmail,
                // Stripe calls the URLs below when certain checkout events happen such as success and failure.
                SuccessUrl = $"{ArticleAdventureFolderManager.GetClientPath()}/Stripe/{UrlSuccess}/sup?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
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
                        UnitAmount = (long)(price * 100),
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = title,
                            Description = description
                        },


                    },
                    Quantity = 1,
                },
            },
                Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
            };
            return options;
        }
        public async Task CheckoutSuccessMainArticle(string sessionId)
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                IUserProfileRepository userProfileRepository =
                          _identityRepositoriesFactory.NewUserProfileRepository(connection);

                List<StripePayment> stripePayments = new List<StripePayment>();

                var sessionService = new SessionService();
                Session session = sessionService.Get(sessionId);
                var amountTotal = session.AmountTotal.Value;
                var Name = session.CustomerDetails.Name;
                var customerEmail = session.CustomerDetails.Email;

                Dictionary<string, string> metaData = session.Metadata;
                var keys = metaData.Keys.ToList();

                var MainArticleId = metaData["IDMainArticle"];
                var NetUidUser = metaData["NetUidUser"];
                metaData.Remove("IDMainArticle");
                metaData.Remove("NetUidUser");
                var stripeRepositoryFactory = _stripeRepositoryFactory.New(connection);

                UserProfile userProfile = userProfileRepository.Get(Guid.Parse(NetUidUser));

                foreach (var idSupArticle in metaData.Values.ToList())
                {
                    StripePayment s = stripeRepositoryFactory.GetPayment(long.Parse(MainArticleId), long.Parse(idSupArticle), userProfile.Id);
                    stripePayments.Add(s);
                }

                foreach (var payment in stripePayments)
                {
                    stripeRepositoryFactory.SetStatusPayment(payment.NetUid);
                }

            }
        }
        public async Task CheckoutSuccessCartArticle(string sessionId)
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                IUserProfileRepository userProfileRepository =
                          _identityRepositoriesFactory.NewUserProfileRepository(connection);

                List<StripePayment> stripePayments = new List<StripePayment>();
                List<MainArticle> mainArticles = new List<MainArticle>();



                var sessionService = new SessionService();
                Session session = sessionService.Get(sessionId);
                var amountTotal = session.AmountTotal.Value;
                var Name = session.CustomerDetails.Name;
                var customerEmail = session.CustomerDetails.Email;

                Dictionary<string, string> metaData = session.Metadata;
                var keys = metaData.Keys.ToList();

                var NetUidUser = metaData["NetUidUser"];
                metaData.Remove("NetUidUser");

                foreach (var MainArticleId in metaData.Values.ToList())
                {
                    var mainArticle = _mainArticleRepositoryFactory.New(connection).GetArticle(long.Parse(MainArticleId));
                    mainArticles.Add(mainArticle);
                }

                var valuesSupArticles = metaData.Values.ToList();
                var stripeRepositoryFactory = _stripeRepositoryFactory.New(connection);

                UserProfile userProfile = userProfileRepository.Get(Guid.Parse(NetUidUser));

                foreach (var mainArticle in mainArticles)
                {
                    foreach (var supArticle in mainArticle.Articles)
                    {
                        StripePayment payment = stripeRepositoryFactory.GetPayment(mainArticle.Id, supArticle.Id, userProfile.Id);
                        stripePayments.Add(payment);
                    }
                }

                //foreach (var MainArticleId in metaData.Values.ToList())
                //{
                //    StripePayment s = stripeRepositoryFactory.GetPayment(long.Parse(), long.Parse(MainArticleId), userProfile.Id);
                //    stripePayments.Add(s);
                //}

                foreach (var payment in stripePayments)
                {
                    stripeRepositoryFactory.SetStatusPayment(payment.NetUid);
                }

            }
        }

        public async Task<CheckoutOrderResponse> CheckOutBuyCartMainArticle(List<MainArticle> mainArticles, string userEmail)
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                IUserProfileRepository userProfileRepository =
                           _identityRepositoriesFactory.NewUserProfileRepository(connection);

                UserProfile userProfile = userProfileRepository.Get(userEmail);
                var stripeRepository = _stripeRepositoryFactory.New(connection);

                List<StripePayment> stripePayments = new List<StripePayment>();

                List<StripePayment> checkStripePayments = new List<StripePayment>();

                List<SessionLineItemOptions> listSession = new List<SessionLineItemOptions>();

                foreach (var mainArticle in mainArticles)
                {
                    foreach (var supArticle in mainArticle.Articles)
                    {
                        var Payments = stripeRepository.GetPayment(mainArticle.Id, supArticle.Id, userProfile.Id);
                        if (Payments != null)
                        {
                            checkStripePayments.Add(Payments);
                        }
                    }
                    var sessionLineItemOptions = new SessionLineItemOptions()
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(mainArticle.Price * 100),
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = mainArticle.Title,
                                Description = mainArticle.Description
                            },


                        },
                        Quantity = 1,
                    };
                    listSession.Add(sessionLineItemOptions);


                }
                StripeCustomer stripeCustomer = new StripeCustomer
                {
                    UserId = userProfile.Id,
                    Name = userProfile.UserName + userProfile?.SurName,
                    Email = userProfile.Email,
                };

                var metaData = new Dictionary<string, string>()
                {
                   { "NetUidUser", userProfile.NetUid.ToString() }
                };

                for (int i = 0; i < mainArticles.Count; i++)
                {
                    metaData.Add($"IDMainArticle{i + 1}", mainArticles[i].Id.ToString());
                }
                var options = new SessionCreateOptions
                {
                    Metadata = metaData,
                    CustomerEmail = userEmail,
                    // Stripe calls the URLs below when certain checkout events happen such as success and failure.
                    SuccessUrl = $"{ArticleAdventureFolderManager.GetClientPath()}/Stripe/SuccessBuyCart?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
                    CancelUrl = $"{ArticleAdventureFolderManager.GetServerPath()}/api/v1/stripe/checkout/failed",  // Checkout cancelled.
                    PaymentMethodTypes = new List<string> // Only card available in test mode?
                {
                    "card"
                },
                    //Metadata = metaData,
                    LineItems = listSession,
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
                foreach (var mainArticle in mainArticles)
                {
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

                }
                foreach (var payment in stripePayments)
                {
                    if (!checkStripePayments.Any(x => x.SupArticleId.Equals(payment.SupArticleId) && x.MainArticleId.Equals(payment.MainArticleId)))
                    {
                        stripeRepository.AddPayment(payment);
                    }
                }

                return checkoutOrderResponse;
            }
        }

        private static SessionCreateOptions SessionCreateCart(string userEmail, List<SessionLineItemOptions> listSession, Dictionary<string, string> metaData)
        {
            var options = new SessionCreateOptions
            {
                Metadata = metaData,
                CustomerEmail = userEmail,
                // Stripe calls the URLs below when certain checkout events happen such as success and failure.
                SuccessUrl = $"{ArticleAdventureFolderManager.GetClientPath()}/Stripe/SuccessBuyСart?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
                CancelUrl = $"{ArticleAdventureFolderManager.GetServerPath()}/api/v1/stripe/checkout/failed",  // Checkout cancelled.
                PaymentMethodTypes = new List<string> // Only card available in test mode?
                {
                    "card"
                },
                //Metadata = metaData,
                LineItems = listSession,
                Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
            };
            return options;
        }

        public async Task<CheckoutOrderResponse> CheckOutBuyNowSupArticle(AuthorArticle supArticle, string userEmail)
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                IUserProfileRepository userProfileRepository =
                           _identityRepositoriesFactory.NewUserProfileRepository(connection);

                UserProfile userProfile = userProfileRepository.Get(userEmail);
                var stripeRepository = _stripeRepositoryFactory.New(connection);

                StripePayment checkStripePayments = new StripePayment();


                var payment = stripeRepository.GetPayment(supArticle.MainArticleId, supArticle.Id, userProfile.Id);

                StripeCustomer stripeCustomer = new StripeCustomer
                {
                    UserId = userProfile.Id,
                    Name = userProfile.UserName + userProfile?.SurName,
                    Email = userProfile.Email,
                };

                var metaData = new Dictionary<string, string>()
                {

                   { "IDSupArticle", supArticle.Id.ToString() },
                   { "IDMainArticle", supArticle.MainArticleId.ToString() },
                   { "NetUidUser", userProfile.NetUid.ToString() }
                };



                SessionCreateOptions options = SessionCreate(supArticle.Price, supArticle.Title, supArticle.Description, userEmail, metaData, "SuccessBuySup");

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                var pubKey = "pk_test_51NdTsYLwAUt6I3BOPOMdxTKOoBPd9UErIyY7J7NY4XwOTDHHfmeFGbcQK18m4KJ6x1g9UZ5TizySV8TxnhamzQbS00cbtKuMRf";

                var checkoutOrderResponse = new CheckoutOrderResponse()
                {
                    SessionId = session.Id,
                    PubKey = pubKey
                };

                StripePayment stripePayment = new StripePayment
                {
                    MainArticleId = supArticle.MainArticleId,
                    SupArticleId = supArticle.Id,
                    PaymentStatus = session.PaymentStatus,
                    ReceiptEmail = userProfile.Email,
                    UserId = userProfile.Id,
                    Description = supArticle.Description,
                    Currency = "USD",
                    Amount = supArticle.Price,
                };

                if (payment == null)
                {
                    stripeRepository.AddPayment(stripePayment);
                }

                return checkoutOrderResponse;
            }
        }

        public async Task CheckoutSuccessSupArticle(string sessionId)
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                IUserProfileRepository userProfileRepository =
                          _identityRepositoriesFactory.NewUserProfileRepository(connection);

                StripePayment stripePayments = new StripePayment();

                var sessionService = new SessionService();
                Session session = sessionService.Get(sessionId);


                var amountTotal = session.AmountTotal.Value;
                var name = session.CustomerDetails.Name;
                var customerEmail = session.CustomerDetails.Email;

                Dictionary<string, string> metaData = session.Metadata;
                var keys = metaData.Keys.ToList();

                var mainArticleId = metaData["IDMainArticle"];
                var supArticleId = metaData["IDSupArticle"];
                var netUidUser = metaData["NetUidUser"];

                var valuesSupArticles = metaData.Values.ToList();
                var stripeRepositoryFactory = _stripeRepositoryFactory.New(connection);

                UserProfile userProfile = userProfileRepository.Get(Guid.Parse(netUidUser));


                StripePayment stripePayment = stripeRepositoryFactory.GetPayment(long.Parse(mainArticleId), long.Parse(supArticleId), userProfile.Id);

                stripeRepositoryFactory.SetStatusPayment(stripePayment.NetUid);

            }
        }


    }
}
