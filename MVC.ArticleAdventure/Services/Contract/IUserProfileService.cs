using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IUserProfileService
    {
        public Task<ExecutionResult<UserProfile>> CreateAccount(RegisterModel userProfile);

        Task<bool> EmailConformation(string token, string userId);
    }
}
