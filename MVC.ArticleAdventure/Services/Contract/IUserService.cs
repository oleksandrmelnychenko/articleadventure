using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using Microsoft.AspNetCore.Http;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IUserService
    {
        public Task<ExecutionResult<UserProfile>> ChangeAccountInformation(UserProfile userProfile,IFormFile formFile, string tokenUser);
        public Task<ExecutionResult<UserProfile>> ChangePassword(Guid userProfileNetUid, string newPassword, string oldPassword, string tokenUser);
        public Task<ExecutionResult<UserProfile>> ChangeEmail(Guid userProfileNetUid, string newEmail, string password, string tokenUser);
        public Task<ExecutionResult<long>> SetFavoriteArticle(Guid userProfileNetUid, Guid MainArtilceNetUid, string tokenUser);
        public Task<ExecutionResult<List<FavoriteArticle>>> GetAllFavoriteArticle(Guid userProfileNetUid, string tokenUser);
        Task<ExecutionResult<FavoriteArticle>> GetFavoriteArticle(Guid userProfileNetUid, Guid MainArtilceNetUid, string tokenUser);
        public Task<ExecutionResult<long>> RemoveFavoriteArticle(Guid netUidFavoriteArticle,string tokenUser);
    }
}
