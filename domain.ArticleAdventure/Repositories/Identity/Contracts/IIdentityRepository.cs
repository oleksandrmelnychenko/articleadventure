using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Identity.Contracts
{
    public interface IIdentityRepository
    {
        Task<Tuple<ClaimsIdentity, User>> AuthAndGetClaimsIdentity(string email, string password);

        Task<Tuple<ClaimsIdentity, User>> AuthAndGetClaimsIdentity(string userId);

        Task<IEnumerable<Claim>> AuthAndGetClaimsIdentity(User user);

        Task Create(UserProfile userProfile, string password);

        Task UpdatePassword(Guid userNetId, string newPassword);

        Task UpdatePassword(User user, string newPassword);

        Task<string> GetUserIdByUserNetId(Guid userNetId);
        Task<User> FindByIdAsync(string userid);
        Task<User> GetUserByEmail(string email);
        Task<string> GenerateEmailConfirmationToken(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<User> GetUserByUserName(string username);

        Task<User> GetUserByUserNetId(Guid userNetId);

        Task ResetPassword(Guid userNetId, Guid resetToken, string newPassword);

        Task UpdateUsersEmail(User user, string email);

        Task UpdateUser(User user);

        Task UpdateUsersUserName(User user, string username);

        void UpdateUsersDisplayName(User user, string username);

        Task ReAssignUsersRole(User user, bool grantAdministrativePermissions);

        Task Delete(Guid userNetId);
    }
}
