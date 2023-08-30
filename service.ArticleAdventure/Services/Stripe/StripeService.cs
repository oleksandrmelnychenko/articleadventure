﻿using domain.ArticleAdventure.Entities;
using service.ArticleAdventure.Services.Stripe.Contracts;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public StripeService(
            ChargeService chargeService,
            CustomerService customerService,
            TokenService tokenService)
        {
            _chargeService = chargeService;
            _customerService = customerService;
            _tokenService = tokenService;
        }

        public async Task<CheckoutOrderResponse> CheckOutBuyNow(MainArticle mainArticle, string thisApiUrl, string userEmail)
        {

            var metaData = new Dictionary<string, string>()
                                {
                                   { "IDArticle", mainArticle.Id.ToString() },
                                   { "IDUser", mainArticle.Id.ToString() }
                                };
            var options = new SessionCreateOptions
            {
                Metadata = metaData,
                CustomerEmail = userEmail,
                // Stripe calls the URLs below when certain checkout events happen such as success and failure.
                SuccessUrl = $"{thisApiUrl}/api/v1/stripe/checkout/success?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
                CancelUrl = $"{thisApiUrl}/api/v1/stripe/checkout/failed" ,  // Checkout cancelled.
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
                        UnitAmount = mainArticle.Price * 100, // Price is in USD cents. 
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = mainArticle.Title,
                            Description = mainArticle.Description,
                            //Metadata = metaData
                            Images = new List<string> { mainArticle.ImageUrl }
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
            return checkoutOrderResponse;

        }

        public async Task CheckoutSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            Session session = sessionService.Get(sessionId);
            var paymentService = new PaymentIntentService();
            PaymentIntent x = paymentService.Get(session.PaymentIntentId);
            
            // Here you can save order and customer details to your database.
            var total = session.AmountTotal.Value;
            //session.
            var g = session.CustomerDetails.Name;
            var customerEmail = session.CustomerDetails.Email;
        }
    }
}
 