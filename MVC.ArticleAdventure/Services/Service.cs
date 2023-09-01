using System.Text.Json;
using System.Text;
using MVC.ArticleAdventure.Extensions;
using common.ArticleAdventure.ResponceBuilder;
using Azure;
using domain.ArticleAdventure.Entities;
using Newtonsoft.Json;

namespace MVC.ArticleAdventure.Services
{
    public abstract class Service
    {
        protected StringContent GetContent(object data)
        {
            return new StringContent(
                System.Text.Json.JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");
        }

        protected async Task<T> DeserializeObjectResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return System.Text.Json.JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected async Task<T> DeserializeResponse<T>(HttpResponseMessage responseMessage)
        {
            var successResponse = await responseMessage.Content.ReadFromJsonAsync<SuccessResponse>();
            var orderResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(successResponse.Body.ToString());
            return orderResponse;
        }
        protected bool CustomContainErrorResponse(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }

        protected async Task<T> DeserializeResponceObject<T> (HttpResponseMessage responseMessage)
        {
            SuccessResponse? Response = await responseMessage.Content.ReadFromJsonAsync<SuccessResponse>();
                return JsonConvert.DeserializeObject<T>(Response.Body.ToString());
        }
    }
}
