using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.EntityHelpers.Identity;
using domain.ArticleAdventure.Models;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json;
using System;
using System.Net;

namespace MVC.ArticleAdventure.Services
{
    public class AuthService : Service, IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthUrl);

            _httpClient = httpClient;
        }

        public async Task<ExecutionResult<CompleteAccessToken>> Login(UserLogin userLogin)
        {
            var result = new ExecutionResult<CompleteAccessToken>();

            try
            {
                var loginContent = GetContent(userLogin);

                var response = await _httpClient.GetAsync($"/api/v1/usermanagement/token/request?email={userLogin.Email}&password={userLogin.Password}&rememberUser=True");
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<CompleteAccessToken>(successResponse.Body.ToString());
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

        public async Task<ExecutionResult<UserProfile>> GetProfileArticles(Guid guid)
        {
            var result = new ExecutionResult<UserProfile>();

            var response = await _httpClient.GetAsync($"/api/v1/usermanagement/get/articles/netuid?userNetId={guid}");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<UserProfile>(successResponse.Body.ToString());
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

        public async Task<ExecutionResult<UserProfile>> GetProfile(Guid guid)
        {
            var result = new ExecutionResult<UserProfile>();

            var response = await _httpClient.GetAsync($"/api/v1/usermanagement/get/netuid?userNetId={guid}");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<UserProfile>(successResponse.Body.ToString());
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

        public async Task<ExecutionResult<CompleteAccessToken>> RefreshToken(string refreshToken)
        {
            var result = new ExecutionResult<CompleteAccessToken>();

            var response = await _httpClient.GetAsync($"/api/v1/usermanagement/token/refresh?refreshToken={refreshToken}");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<CompleteAccessToken>(successResponse.Body.ToString());
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

        public async Task<ExecutionResult<List<UserProfile>>> GetAllProfile(string tokenAdmin)
        {
            var result = new ExecutionResult<List<UserProfile>>();

            var response = await _httpClient.GetAsync($"/api/v1/usermanagement/get/all");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<List<UserProfile>>(successResponse.Body.ToString());
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
    }

   

    

   
}
