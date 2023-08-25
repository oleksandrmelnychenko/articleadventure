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

           var response = await _httpClient.PostAsJsonAsync($"{PathTag.BUY_STRIPE}?emailUser={Email}", mainArticle);

            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            CheckoutOrderResponse orderResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckoutOrderResponse>(successResponse.Body.ToString());
            return orderResponse;
            //var loginContent = GetContent(userLogin);
            ////request token
            //var response = await _httpClient.GetAsync($"/api/v1/usermanagement/token/request?email={userLogin.Email}&password={userLogin.Password}&rememberUser=True");

            //if (!CustomContainErrorResponse(response))
            //{
            //    return new UserResponseLogin
            //    {
            //        ResponseResult = await DeserializeObjectResponse<ErrorResponse>(response)
            //    };
            //}
            //else
            //{
            //    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            //    UserResponseLogin userResponseLogin = Newtonsoft.Json.JsonConvert.DeserializeObject<UserResponseLogin>(successResponse.Body.ToString());
            //    return userResponseLogin;
            //}


        }
    }
}
