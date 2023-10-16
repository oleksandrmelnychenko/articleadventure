using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;

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

        public async Task<CheckoutOrderResponse> BuyStripeMainArticle(MainArticle mainArticle, string Email, string tokenUser)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenUser);
            var response = await _httpClient.PostAsJsonAsync($"{PathStripe.BUY_NOW_STRIPE_MAIN_ARTICLE}?emailUser={Email}", mainArticle);

            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            CheckoutOrderResponse orderResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckoutOrderResponse>(successResponse.Body.ToString());
            return orderResponse;
        }

        public async Task<ExecutionResult<CheckoutOrderResponse>> BuyStripeSupArticle(AuthorArticle mainArticle, string Email, string tokenUser)
        {
            var result = new ExecutionResult<CheckoutOrderResponse>();

            var response = await _httpClient.PostAsJsonAsync($"{PathStripe.BUY_NOW_STRIPE_SUP_ARTICLE}?emailUser={Email}", mainArticle);
            try
            {
                if (response.IsSuccessStatusCode) 
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<CheckoutOrderResponse>(successResponse.Body.ToString());
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    result.Error = new ErrorMVC();
                    result.Error.StatusCode = errorResponse.StatusCode;
                    result.Error.Message = errorResponse.Message;
                }
            }
            catch (Exception e)
            {

                result.Error.Message = e.Message;
            }
            return result;
        }

        public async Task<ExecutionResult<CheckoutOrderResponse>> BuyStripeCartArticle(List<MainArticle> mainArticle, string Email, string tokenUser)
        {
            var result = new ExecutionResult<CheckoutOrderResponse>();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenUser);
            var response = await _httpClient.PostAsJsonAsync($"{PathStripe.BUY_NOW_STRIPE_CART_ARTICLE}?emailUser={Email}", mainArticle);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<CheckoutOrderResponse>(successResponse.Body.ToString());
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    result.Error = new ErrorMVC();
                    result.Error.StatusCode = errorResponse.StatusCode;
                    result.Error.Message = errorResponse.Message;
                }
            }
            catch (Exception e)
            {

                result.Error.Message = e.Message;
            }
            return result;
        }

        public async Task<ExecutionResult<List<StripePayment>>> CheckPaymentsHaveUser(string userEmail)
        {
            var result = new ExecutionResult<List<StripePayment>>();

            var response = await _httpClient.GetAsync($"{PathStripe.CHECK_PAYMENTS_HAVE_USER}?userMail={userEmail}");
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<List<StripePayment>>(successResponse.Body.ToString());
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    result.Error = new ErrorMVC();
                    result.Error.StatusCode = errorResponse.StatusCode;
                    result.Error.Message = errorResponse.Message;
                }
            }
            catch (Exception e)
            {

                result.Error.Message = e.Message;
            }
            return result;
        }

        public async Task CheckoutSuccess(string sessionId)
        {
            var response = await _httpClient.GetAsync($"{PathStripe.CHECK_SUCCESS}?sessionId={sessionId}");
        }
        public async Task CheckoutSuccessSup(string sessionId)
        {
            var response = await _httpClient.GetAsync($"{PathStripe.CHECK_SUCCESS_SUP}?sessionId={sessionId}");
        }

        public async Task CheckoutSuccessCart(string sessionId)
        {
            var response = await _httpClient.GetAsync($"{PathStripe.CHECK_SUCCESS_CART}?sessionId={sessionId}");
        }
    }
}
 