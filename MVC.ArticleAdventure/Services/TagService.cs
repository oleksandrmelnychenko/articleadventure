using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json;

namespace MVC.ArticleAdventure.Services
{
    public class TagService:Service,ITagService
    {
        private readonly HttpClient _httpClient;

        public TagService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthUrl);

            _httpClient = httpClient;
        }
        public async Task AddMainTag(MainTag mainTag)
        {
            await _httpClient.PostAsJsonAsync(PathTag.ADD_MAIN_TAG, mainTag);
        }

        public async Task AddSupTag(SupTag supTag)
        {
            await _httpClient.PostAsJsonAsync(PathTag.ADD_SUP_TAG, supTag);
        }

        public async Task ChangeMainTag(MainTag mainTag)
        {
            await _httpClient.PostAsJsonAsync(PathTag.CHANGE_MAIN_TAG, mainTag);
        }

        public async Task ChangeSupTag(SupTag supTag)
        {
            await _httpClient.PostAsJsonAsync(PathTag.CHANGE_SUP_TAG, supTag);
        }

        public async Task<List<MainTag>> GetAllTags()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(PathTag.GET_ALL_MAIN_TAG);

            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            List<MainTag> allMainTags = JsonConvert.DeserializeObject<List<MainTag>>(successResponse.Body.ToString());
            return allMainTags;
        }

        public async Task<MainTag> GetMainTag(Guid NetUidMainTag)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{PathTag.GET_MAIN_TAG}?netUidMainTag={NetUidMainTag}");

            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            MainTag mainTag = JsonConvert.DeserializeObject<MainTag>(successResponse.Body.ToString());
            return mainTag;
        }

        public async Task<SupTag> GetSupTag(Guid NetUidSupTag)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{PathTag.GET_SUP_TAG}?netUidSupTag={NetUidSupTag}");

            var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
            SupTag mainTag = JsonConvert.DeserializeObject<SupTag>(successResponse.Body.ToString());
            return mainTag;
        }

        public async Task RemoveMainTag(Guid NetUidMainTag)
        {
            await _httpClient.GetAsync($"{PathTag.REMOVE_MAIN_TAG}?netUidMainTag={NetUidMainTag}");
        }

        public async Task RemoveSupTag(Guid NetUidSupTag)
        {
           await _httpClient.GetAsync($"{PathTag.REMOVE_SUP_TAG}?netUidSupTag={NetUidSupTag}");
        }
    }
}
