using domain.ArticleAdventure.Models;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IAuthService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);

        Task<UserResponseLogin> Register(UserRegister userRegister);
    }
}
