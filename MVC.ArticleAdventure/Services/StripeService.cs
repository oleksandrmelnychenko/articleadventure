using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;

namespace MVC.ArticleAdventure.Services
{
    public class StripeService:Service,IStripeService
    {
        private readonly HttpClient _httpClient;

        public StripeService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthUrl);

            _httpClient = httpClient;
        }

        public async Task<CheckoutOrderResponse> BuyStripe(MainArticle mainArticle, string Email)
        {

           var response = await _httpClient.PostAsJsonAsync($"{PathStripe.BUY_NOW_STRIPE}?emailUser={Email}", mainArticle);

            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            CheckoutOrderResponse orderResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckoutOrderResponse>(successResponse.Body.ToString());
            return orderResponse;
        }

        public async Task<List<StripePayment>> CheckPaymentsHaveUser(string userEmail)
        {

            var response = await _httpClient.GetAsync($"{PathStripe.CHECK_PAYMENTS_HAVE_USER}?userMail={userEmail}");
           var payments = await DeserializeResponse<List<StripePayment>>(response);
            return payments;
        }

        public async Task CheckoutSuccess(string sessionId)
        {
            var response = await _httpClient.GetAsync($"{PathStripe.CHECK_SUCCESS}?sessionId={sessionId}");

            
        }
    }
}
