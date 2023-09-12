using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services.UserManagement.Contracts
{
    public interface IUserProfileService
    {
        Task<UserProfile> Create(UserProfile userProfile, string password);
        Task<UserProfile> FullUpdate(UserProfile userProfile, string password);
        Task<UserProfile> GetById(Guid userNetId);
        Task<IdentityResult> ConforimEmail(string token, string userId);

        Task<UserProfile> UpdatePassword(Guid userProfileNetUid, string password,string oldPassword);
        Task<UserProfile> UpdateEmail(Guid userProfileNetUid, string Email,string password);
        Task<UserProfile> UpdateAccountInformation(UserProfile userProfile);
        Task<long> SetFavoriteArticle(Guid netUidArticle , Guid netUidUser);
        Task<FavoriteArticle> GetFavoriteArticle(Guid netUidArticle, Guid netUidUser);
        Task<List<FavoriteArticle>> GetAllFavoriteArticle( Guid userProfileNetUid);
        Task<long> RemoveFavoriteArticle(Guid netUidFavoriteArticle);

    }
}
