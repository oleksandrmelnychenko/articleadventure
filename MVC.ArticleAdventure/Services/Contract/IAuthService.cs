using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.EntityHelpers.Identity;
using domain.ArticleAdventure.Models;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IAuthService
    {
        Task<ExecutionResult<CompleteAccessToken>> Login(UserLogin userLogin);//no
        Task<ExecutionResult<CompleteAccessToken>> RefreshToken(string refreshToken);//no
        Task<ExecutionResult<UserProfile>> GetProfile(Guid guid);//no Admin
        Task<ExecutionResult<List<UserProfile>>> GetAllProfile(string tokenAdmin);//token Admin
        Task<ExecutionResult<UserProfile>> GetProfileArticles(Guid guid);//no token
    }
}
