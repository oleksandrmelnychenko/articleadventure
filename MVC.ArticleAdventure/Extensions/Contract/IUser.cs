using System.Security.Claims;

namespace MVC.ArticleAdventure.Extensions.Contract
{
    public interface IUser
    {
        string Name { get; }

        Guid GetUserId();

        string GetUserEmail();

        string GetUserToken();

        bool IsAuthenticate();

        bool ContainRole(string role);

        IEnumerable<Claim> GetClaims();

        HttpContext GetHttpContext();
    }
}
