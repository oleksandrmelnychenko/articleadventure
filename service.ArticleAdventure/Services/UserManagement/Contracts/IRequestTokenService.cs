using domain.ArticleAdventure.EntityHelpers.Identity;
using domain.ArticleAdventure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services.UserManagement.Contracts
{
    public interface IRequestTokenService
    {
        Task<CompleteAccessToken> RequestToken(string userName, string password, bool rememberUser);

        Task<CompleteAccessToken> RefreshToken(string refreshToken);

        Task DeleteRefreshTokenOnLogoutByUserId(Guid userNetId);
    }
}
