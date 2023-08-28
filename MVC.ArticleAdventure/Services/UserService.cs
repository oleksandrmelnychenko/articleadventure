using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
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
        public async Task ChangeAccountInformation(UserProfile userProfile)
        {
            var response = await _httpClient.PostAsJsonAsync(PathUser.UPDATE_ACCOUNT_INFORMATION, userProfile);

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
    }
} 
