using System.Security.Claims;

namespace MVC.ArticleAdventure.Helpers
{
    public static class UserRoleHelper
    {
        public static bool IsUserRole(IEnumerable<Claim> claims, string role)
        {
            string value = claims.FirstOrDefault(x => x.Type == "role").Value;

            if (value == null)
            {
                return false;
            }

            return value == role ? true : false;
        }
    }
}
