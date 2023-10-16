using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IUserProfileService
    {
        public Task<ExecutionResult<UserProfile>> CreateAccount(RegisterModel userProfile);
        public Task<ExecutionResult<UserProfile>> RemoveAccount(Guid netUidAccount);//token Admin
        Task<ExecutionResult<UserProfile>> FullUpdate(RegisterModel registerModel);

        Task<bool> EmailConformation(string token, string userId);
    }
}
