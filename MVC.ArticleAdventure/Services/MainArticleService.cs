using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.IdentityEntities;
using Microsoft.Extensions.Options;
using MVC.ArticleAdventure.Extensions;
using MVC.ArticleAdventure.Extensions.Contract;
using MVC.ArticleAdventure.Helpers;
using MVC.ArticleAdventure.Services.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;

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
        public async Task<ExecutionResult<long>> AddArticle(MainArticle article, IFormFile photoMainArticle)
        {
            var result = new ExecutionResult<long>();

            try
            {
                using var form = new MultipartFormDataContent();

                var jsonArticle = JsonConvert.SerializeObject(article);
                var stringContentArticle = new StringContent(jsonArticle, Encoding.UTF8, "application/json");
                form.Add(stringContentArticle, "article");

                using var streamContent = new StreamContent(photoMainArticle.OpenReadStream());
                form.Add(streamContent, "filePhotoMainArticle", photoMainArticle.FileName);
                var response = await _httpClient.PostAsync(PathMainArticle.ADD_ARTICLE, form);


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
        public async Task<ExecutionResult<AuthorArticle>> GeSupArticle(Guid netUidArticle){
            var result = new ExecutionResult<AuthorArticle>();

            try
            {
                var response = await _httpClient.GetAsync($"{PathMainArticle.GET_SUP_ARTICLE}?netUidArticle={netUidArticle}");

                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<AuthorArticle>(successResponse.Body.ToString());
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
        public async Task<ExecutionResult<List<MainArticle>>> GetAllArticlesFilterSupTags(List<MainArticleTags> mainArticleTags)
        {
            var result = new ExecutionResult<List<MainArticle>>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(PathMainArticle.GET_ALL_ARTICLE_FILTER_SUP_TAGS, mainArticleTags);

                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<List<MainArticle>>(successResponse.Body.ToString());
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

        public async Task<ExecutionResult<List<MainArticle>>> GetAllFilterDateTimeArticles()
        {
            var result = new ExecutionResult<List<MainArticle>>();

            try
            {
                var response = await _httpClient.GetAsync(PathMainArticle.ALL_ARTICLE_FILTERED_DATE_TIME);

                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<List<MainArticle>>(successResponse.Body.ToString());
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

        public async Task<ExecutionResult<MainArticle>> GetArticleUser(Guid netUidArticle,long idUser)
        {
            var result = new ExecutionResult<MainArticle>();

            try
            {
                var response = await _httpClient.GetAsync($"{PathMainArticle.GET_ARTICLE_USER}?netUidArticle={netUidArticle}&idUser={idUser}");

                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<MainArticle>(successResponse.Body.ToString());
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

        public async Task<MainArticle> GetArticle(long id)
        {
            var response = await _httpClient.GetAsync($"{PathMainArticle.GET_ARTICLE}?id={id}");

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

        public async Task<ExecutionResult<long>> Update(MainArticle article, IFormFile photoMainArticle)
        {
            var result = new ExecutionResult<long>();
            HttpResponseMessage response;
            try
            {
                using var form = new MultipartFormDataContent();
                var jsonArticle = JsonConvert.SerializeObject(article);
                var stringContentArticle = new StringContent(jsonArticle, Encoding.UTF8, "application/json");
                form.Add(stringContentArticle, "article");
                if (photoMainArticle != null)
                {
                    using (var streamContent = new StreamContent(photoMainArticle.OpenReadStream()))
                    {
                        form.Add(streamContent, "filePhotoMainArticle", photoMainArticle.FileName);
                        response = await _httpClient.PostAsync(PathMainArticle.UPDATE_ARTICLE, form);
                        
                    }

                }
                else
                {
                     response = await _httpClient.PostAsync(PathMainArticle.UPDATE_ARTICLE, form);
                }


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

        public async Task<ExecutionResult<List<MainArticle>>> GetAllArticlesUser(long idUser)
        {
            var result = new ExecutionResult<List<MainArticle>>();

            HttpResponseMessage response = await _httpClient.GetAsync($"{PathMainArticle.GET_ALL_ARTICLE_USER}?idUser={idUser}");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<List<MainArticle>>(successResponse.Body.ToString());
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

        public async Task<ExecutionResult<List<StripePayment>>> GetAllStripePaymentsUser(long idUser)
        {
            var result = new ExecutionResult<List<StripePayment>>();

            HttpResponseMessage response = await _httpClient.GetAsync($"{PathMainArticle.GET_USER_ALL_STRIPE_PAYMENTS}?idUser={idUser}");

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();
                    result.Data = JsonConvert.DeserializeObject<List<StripePayment>>(successResponse.Body.ToString());
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
