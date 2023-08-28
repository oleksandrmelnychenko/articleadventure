using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface IUserService
    {
        public Task ChangeAccountInformation(UserProfile userProfile);

        public Task ChangePassword(Guid userProfileNetUid, string newPassword, string oldPassword);
        public Task ChangeEmail(Guid userProfileNetUid, string newEmail, string password);
    }
}
