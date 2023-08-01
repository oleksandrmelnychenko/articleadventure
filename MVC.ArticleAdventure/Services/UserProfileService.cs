using Azure.Core;
using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.IdentityEntities;
using domain.ArticleAdventure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Web;

namespace MVC.ArticleAdventure.Services
{
    public class UserProfileService : Service, IUserProfileService
    {
        private readonly HttpClient _httpClient;

        public UserProfileService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthUrl);

            _httpClient = httpClient;
        }
        public async Task<UserResponseLogin> CreateAccount(RegisterModel registerModel)
        {
            UserProfile userProfile = new UserProfile { Email = registerModel.Email, UserName = registerModel.UserName };
            var response = await _httpClient.PostAsJsonAsync($"{PathUserProfile.CREATE_USER}/?password={registerModel.Password}", userProfile);

            if (!CustomContainErrorResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeObjectResponse<ErrorResponse>(response)
                };
            }
            else
            {
                var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                UserProfile userResponseLogin = JsonConvert.DeserializeObject<UserProfile>(successResponse.Body.ToString());
                return new UserResponseLogin();
            }
        }
        public async Task<bool> EmailConformation(string token,string userId)
        {
            string codeHtmlVersion = HttpUtility.UrlEncode(token);
            var response = await _httpClient.GetAsync($"/api/v1/usermanagement/email?token= " + codeHtmlVersion + "&userId="+ userId);
            if (!CustomContainErrorResponse(response))
            {
                return false;
            }
            else
            {
                var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                IdentityResult result = JsonConvert.DeserializeObject<IdentityResult>(successResponse.Body.ToString());
                return result.Succeeded;
            }
        }
    }
}
