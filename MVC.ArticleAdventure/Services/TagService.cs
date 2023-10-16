using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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
        public async Task AddMainTag(MainTag mainTag, string tokenAdmin)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAdmin);
            await _httpClient.PostAsJsonAsync(PathTag.ADD_MAIN_TAG, mainTag);
        }

        public async Task AddSupTag(SupTag supTag, string tokenAdmin)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAdmin);
            await _httpClient.PostAsJsonAsync(PathTag.ADD_SUP_TAG, supTag);
        }

        public async Task ChangeMainTag(MainTag mainTag, string tokenAdmin)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAdmin);
            await _httpClient.PostAsJsonAsync(PathTag.CHANGE_MAIN_TAG, mainTag);
        }

        public async Task ChangeSupTag(SupTag supTag, string tokenAdmin)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAdmin);
            await _httpClient.PostAsJsonAsync(PathTag.CHANGE_SUP_TAG, supTag);
        }

        public async Task<ExecutionResult<List<MainTag>>> GetAllTags()
        {
            var result = new ExecutionResult<List<MainTag>>();

            HttpResponseMessage response = await _httpClient.GetAsync(PathTag.GET_ALL_MAIN_TAG);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<List<MainTag>>(successResponse.Body.ToString());
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

        public async Task RemoveMainTag(Guid NetUidMainTag, string tokenAdmin)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAdmin);
            await _httpClient.GetAsync($"{PathTag.REMOVE_MAIN_TAG}?netUidMainTag={NetUidMainTag}");
        }

        public async Task RemoveSupTag(Guid NetUidSupTag, string tokenAdmin)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAdmin);
            await _httpClient.GetAsync($"{PathTag.REMOVE_SUP_TAG}?netUidSupTag={NetUidSupTag}");
        }
    }
}
