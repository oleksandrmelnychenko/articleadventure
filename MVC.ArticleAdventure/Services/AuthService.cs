﻿using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Models;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Services.Contract;

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

        public async Task<UserResponseLogin> Login(UserLogin userLogin)
        {
            var loginContent = GetContent(userLogin);
            //request token
            var response = await _httpClient.GetAsync($"/api/v1/usermanagement/token/request?email={userLogin.Email}&password={userLogin.Password}&rememberUser=True");
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!CustomContainErrorResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                };
            }
            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            UserResponseLogin userResponseLogin = Newtonsoft.Json.JsonConvert.DeserializeObject<UserResponseLogin>(successResponse.Body.ToString());

            return userResponseLogin; 
        }

        public async Task<UserResponseLogin> Register(UserRegister userRegister)
        {
            var registerContent = GetContent(userRegister);

            var response = await _httpClient.PostAsync("/api/identity/RegisterUser", registerContent);

            if (!CustomContainErrorResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                };
            }

            return await DeserializeObjectResponse<UserResponseLogin>(response);
        }
    }
}