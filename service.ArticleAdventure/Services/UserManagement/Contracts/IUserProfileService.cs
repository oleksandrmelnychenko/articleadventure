using domain.ArticleAdventure.Entities;
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
    }
}
