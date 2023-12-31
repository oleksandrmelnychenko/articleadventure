﻿using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.EntityHelpers.Identity;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

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
        public async Task<ExecutionResult<UserProfile>> ChangeAccountInformation(UserProfile userProfile, IFormFile photoUserProfile, string tokenUser)
        {
            var result = new ExecutionResult<UserProfile>();

            try
            {
                using var form = new MultipartFormDataContent();

                var jsonArticle = JsonConvert.SerializeObject(userProfile);
                var stringContentUserProfile = new StringContent(jsonArticle, Encoding.UTF8, "application/json");
                form.Add(stringContentUserProfile, "userProfile");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenUser);

                if (photoUserProfile != null)
                {
                    using var streamContent = new StreamContent(photoUserProfile.OpenReadStream());
                    form.Add(streamContent, "photoUserProfile", photoUserProfile.FileName);
                }
                
                var response = await _httpClient.PostAsync(PathUser.UPDATE_ACCOUNT_INFORMATION, form);


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

        public async Task<ExecutionResult<UserProfile>> ChangeEmail(Guid userProfileNetUid, string newEmail, string password, string tokenUser)
        {
            var result = new ExecutionResult<UserProfile>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenUser);

                var response = await _httpClient.GetAsync($"{PathUser.UPDATE_EMAIL}?userProfileNetUid={userProfileNetUid}&newEmail={newEmail}&password={password}");


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

        public async Task<ExecutionResult<UserProfile>> ChangePassword(Guid userProfileNetUid, string newPassword, string oldPassword, string tokenUser)
        {
            var result = new ExecutionResult<UserProfile>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenUser);

                var response = await _httpClient.GetAsync($"{PathUser.UPDATE_PASSWORD}?userProfileNetUid={userProfileNetUid}&newPassword={newPassword}&oldPassword={oldPassword}");

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

        public async Task<ExecutionResult<long>> SetFavoriteArticle(Guid userProfileNetUid, Guid MainArtilceNetUid, string tokenUser)
        {
            var result = new ExecutionResult<long>();
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenUser);

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
        public async Task<ExecutionResult<FavoriteArticle>> GetFavoriteArticle(Guid userProfileNetUid, Guid MainArtilceNetUid, string tokenUser)
        {
            var result = new ExecutionResult<FavoriteArticle>();
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenUser);

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
        public async Task<ExecutionResult<List<FavoriteArticle>>> GetAllFavoriteArticle(Guid userProfileNetUid, string tokenUser)
        {
            var result = new ExecutionResult<List<FavoriteArticle>>();
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenUser);

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
        public async Task<ExecutionResult<long>> RemoveFavoriteArticle(Guid netUidFavoriteArticle, string tokenUser)
        {
            var result = new ExecutionResult<long>();
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenUser);

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
