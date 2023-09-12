using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.EntityHelpers.Identity;
using domain.ArticleAdventure.Models;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;

namespace MVC.ArticleAdventure.Services
{
    public class UserService : Service, IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthUrl);

            _httpClient = httpClient;
        }
        public async Task<ExecutionResult<UserProfile>> ChangeAccountInformation(UserProfile userProfile)
        {
            var result = new ExecutionResult<UserProfile>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(PathUser.UPDATE_ACCOUNT_INFORMATION, userProfile);
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

        public async Task ChangeEmail(Guid userProfileNetUid,string newEmail,string password) 
        {
            var response = await _httpClient.GetAsync($"{PathUser.UPDATE_EMAIL}?userProfileNetUid={userProfileNetUid}&newEmail={newEmail}&password={password}");

            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            UserProfile userResponseLogin = JsonConvert.DeserializeObject<UserProfile>(successResponse.Body.ToString());
        }

        public async Task ChangePassword(Guid userProfileNetUid, string newPassword,string oldPassword)
        {
            var response = await _httpClient.GetAsync($"{PathUser.UPDATE_PASSWORD}?userProfileNetUid={userProfileNetUid}&newPassword={newPassword}&oldPassword={oldPassword}");

            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            UserProfile userResponseLogin = JsonConvert.DeserializeObject<UserProfile>(successResponse.Body.ToString());
        }

        public async Task<ExecutionResult<long>> SetFavoriteArticle(Guid userProfileNetUid, Guid MainArtilceNetUid)
        {
            var result = new ExecutionResult<long>();
            try
            {
                var response = await _httpClient.GetAsync($"{PathUser.SET_FAVORITE_ARTICLE}?netUidArticle={MainArtilceNetUid}&netUidUser={userProfileNetUid}");
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<long>(successResponse.Body.ToString());
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
        public async Task<ExecutionResult<FavoriteArticle>> GetFavoriteArticle(Guid userProfileNetUid, Guid MainArtilceNetUid)
        {
            var result = new ExecutionResult<FavoriteArticle>();
            try
            {
                var response = await _httpClient.GetAsync($"{PathUser.GET_FAVORITE_ARTICLE}?netUidArticle={MainArtilceNetUid}&netUidUser={userProfileNetUid}");
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    if (successResponse.Body != null)
                    {
                        result.Data = JsonConvert.DeserializeObject<FavoriteArticle>(successResponse.Body.ToString());
                    }
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
        public async Task<ExecutionResult<List<FavoriteArticle>>> GetAllFavoriteArticle(Guid userProfileNetUid)
        {
            var result = new ExecutionResult<List<FavoriteArticle>>();
            try
            {
                var response = await _httpClient.GetAsync($"{PathUser.GET_ALL_FAVORITE_ARTICLE}?userProfileNetUid={userProfileNetUid}");
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<List<FavoriteArticle>>(successResponse.Body.ToString());
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
        public async Task<ExecutionResult<long>> RemoveFavoriteArticle(Guid netUidFavoriteArticle)
        {
            var result = new ExecutionResult<long>();
            try
            {
                var response = await _httpClient.GetAsync($"{PathUser.REMOVE_FAVORITE_ARTICLE}?netUidFavoriteArticle={netUidFavoriteArticle}");
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<long>(successResponse.Body.ToString());
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
