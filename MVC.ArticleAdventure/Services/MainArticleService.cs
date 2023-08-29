using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace MVC.ArticleAdventure.Services
{
    public class MainArticleService : Service, IMainArticleService
    {
        private readonly HttpClient _httpClient;
        public MainArticleService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthUrl);

            _httpClient = httpClient;
        }
        public async Task AddArticle(MainArticle article, IFormFile photoMainArticle)
        {
            using var form = new MultipartFormDataContent();

            // Сериализуем объект article в JSON и добавляем его как контент
            var jsonArticle = JsonConvert.SerializeObject(article);
            var stringContentArticle = new StringContent(jsonArticle, Encoding.UTF8, "application/json");
            form.Add(stringContentArticle, "article");
            
            // Добавляем файл как контент
            using var streamContent = new StreamContent(photoMainArticle.OpenReadStream());
            form.Add(streamContent, "filePhotoMainArticle", photoMainArticle.FileName);

            var response = await _httpClient.PostAsync(PathMainArticle.ADD_ARTICLE, form);
        }

        public async Task<List<MainArticle>> GetAllArticles()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(PathMainArticle.GET_ALL_ARTICLE);

            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            List<MainArticle> userResponseLogin = JsonConvert.DeserializeObject<List<MainArticle>>(successResponse.Body.ToString());
            return userResponseLogin;
        }

        public async Task<MainArticle> GetArticle(Guid netUidArticle)
        {
            var response = await _httpClient.GetAsync($"{PathMainArticle.GET_ARTICLE}?netUidArticle={netUidArticle}");
            
            if (!CustomContainErrorResponse(response))
            {
                return new MainArticle();
            }
            else
            {
                var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                MainArticle authorArticleResponce = JsonConvert.DeserializeObject<MainArticle>(successResponse.Body.ToString());
                return authorArticleResponce;
            }
        }

        public async Task Remove(Guid netUidArticle)
        {
            var response = await _httpClient.GetAsync($"{PathMainArticle.REMOVE_ARTICLE}?netUidArticle={netUidArticle}");
            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
        }

        public async Task Update(MainArticle article, IFormFile photoMainArticle)
        {
            using var form = new MultipartFormDataContent();

            var jsonArticle = JsonConvert.SerializeObject(article);
            var stringContentArticle = new StringContent(jsonArticle, Encoding.UTF8, "application/json");
            form.Add(stringContentArticle, "article");

            using var streamContent = new StreamContent(photoMainArticle.OpenReadStream());
            form.Add(streamContent, "filePhotoMainArticle", photoMainArticle.FileName);

            var response = await _httpClient.PostAsync(PathMainArticle.UPDATE_ARTICLE, form);
        }
    }
}
