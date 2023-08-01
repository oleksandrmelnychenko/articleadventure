using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Models;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IUserProfileService
    {
        public Task<UserResponseLogin> CreateAccount(RegisterModel userProfile);

        Task<bool> EmailConformation(string token, string userId);
    }
}
