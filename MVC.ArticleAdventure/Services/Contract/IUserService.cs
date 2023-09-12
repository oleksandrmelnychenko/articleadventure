using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IUserService
    {
        public Task<ExecutionResult<UserProfile>> ChangeAccountInformation(UserProfile userProfile);

        public Task ChangePassword(Guid userProfileNetUid, string newPassword, string oldPassword);
        public Task ChangeEmail(Guid userProfileNetUid, string newEmail, string password);
        public Task<ExecutionResult<long>> SetFavoriteArticle(Guid userProfileNetUid, Guid MainArtilceNetUid);
        public Task<ExecutionResult<List<FavoriteArticle>>> GetAllFavoriteArticle(Guid userProfileNetUid);
        Task<ExecutionResult<FavoriteArticle>> GetFavoriteArticle(Guid userProfileNetUid, Guid MainArtilceNetUid);
        public Task<ExecutionResult<long>> RemoveFavoriteArticle(Guid netUidFavoriteArticle);
    }
}
