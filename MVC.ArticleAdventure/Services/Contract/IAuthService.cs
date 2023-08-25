using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.EntityHelpers.Identity;
using domain.ArticleAdventure.Models;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IAuthService
    {
        Task<CompleteAccessToken> Login(UserLogin userLogin);

        Task<UserResponseLogin> Register(UserRegister userRegister);

        Task<UserProfile> GetProfile(Guid guid);
    }
}
