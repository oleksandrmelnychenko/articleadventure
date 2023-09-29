using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.EntityHelpers.Identity;
using domain.ArticleAdventure.Models;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IAuthService
    {
        Task<ExecutionResult<CompleteAccessToken>> Login(UserLogin userLogin);
        Task<ExecutionResult<UserProfile>> GetProfile(Guid guid);
        Task<ExecutionResult<List<UserProfile>>> GetAllProfile();
        Task<ExecutionResult<UserProfile>> GetProfileArticles(Guid guid);
    }
}
