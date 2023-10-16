using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json;
using System.Net.Http;
using MVC.ArticleAdventure.Helpers;

namespace MVC.ArticleAdventure.Services
{
    public class ArticleService : Service, IArticleService
    {
        private readonly HttpClient _httpClient;
        public ArticleService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthUrl);

            _httpClient = httpClient;
        }

        public async Task AddArticle(AuthorArticle blogs)
        {
            await _httpClient.PostAsJsonAsync(PathArticle.ADD_ARTICLE, blogs);
        }

        public async Task<List<AuthorArticle>> GetAllArticles()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(PathArticle.GET_ALL_ARTICLE);

            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            List<AuthorArticle> userResponseLogin = JsonConvert.DeserializeObject<List<AuthorArticle>>(successResponse.Body.ToString());
            return userResponseLogin;
        }

        public async Task<AuthorArticle> GetArticle(Guid netUidArticle)
        {
            var response = await _httpClient.GetAsync($"{PathArticle.GET_ARTICLE}?netUidArticle={netUidArticle}");
            
            if (!CustomContainErrorResponse(response))
            {
                return new AuthorArticle();
            }
            else
            {
                var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                AuthorArticle authorArticleResponce = JsonConvert.DeserializeObject<AuthorArticle>(successResponse.Body.ToString());
                return authorArticleResponce;
            }
        }

        public async Task Remove(Guid netUidArticle, string tokenAdmin)
        {
            var response = await _httpClient.GetAsync($"{PathArticle.REMOVE_ARTICLE}?netUidArticle={netUidArticle}");
            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
        }

        public async Task Update(AuthorArticle article, string tokenAdmin)
        {
            var response = await _httpClient.PostAsJsonAsync(PathArticle.UPDATE_ARTICLE, article);
        }
        
    }
}
