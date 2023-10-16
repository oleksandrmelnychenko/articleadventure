using Azure.Core;
using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Helpers;
using domain.ArticleAdventure.IdentityEntities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using domain.ArticleAdventure.Repositories.Identity;
using domain.ArticleAdventure.Repositories.Identity.Contracts;
using domain.ArticleAdventure.Repositories.Stripe.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using service.ArticleAdventure.MailSenderServices;
using service.ArticleAdventure.MailSenderServices.Contracts;
using service.ArticleAdventure.Services.UserManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services.UserManagement
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IIdentityRepositoriesFactory _identityRepositoriesFactory;
        private readonly IMailSenderFactory _mailSenderFactory;
        private readonly IMainArticleRepositoryFactory _mainRepositoryFactory;


        public UserProfileService(
              IDbConnectionFactory connectionFactory,
              IIdentityRepositoriesFactory identityRepositoriesFactory,
              IMailSenderFactory mailSenderFactory,
              IMainArticleRepositoryFactory mainRepositoryFactory)
        {
            _connectionFactory = connectionFactory;
            _identityRepositoriesFactory = identityRepositoriesFactory;
            _mailSenderFactory = mailSenderFactory;
            _mainRepositoryFactory = mainRepositoryFactory;
        }
        public Task<IdentityResult> ConforimEmail(string emailConfirmationToken, string userId) => Task.Run(async () =>
        {
            using (IDbConnection connection = _connectionFactory.NewSqlConnection())
            {
                IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository();
                User findUserById = await identityRepository.FindByIdAsync(userId);
                IdentityResult confirmEmailResult = await identityRepository.ConfirmEmailAsync(findUserById, emailConfirmationToken);

                return confirmEmailResult;
            }
        });
        public Task<UserProfile> Create(UserProfile userProfile, string password) =>
           Task.Run(async () =>
           {
               using (IDbConnection connection = _connectionFactory.NewSqlConnection())
               {
                   if (userProfile == null) throw new Exception("Dev exception. Entity can not be empty");

                   if (string.IsNullOrEmpty(password)) throw new Exception("Password is required");

                   if (string.IsNullOrEmpty(userProfile.Email)) throw new Exception("Email is required");

                   IUserProfileRepository userProfileRepository = _identityRepositoriesFactory.NewUserProfileRepository(connection);

                   UserProfile existingProfile = userProfileRepository.Get(userProfile.Email);

                   if (existingProfile != null) throw new Exception("User with such email is already exists");

                   IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository();

                   userProfile.Id = userProfileRepository.Add(userProfile);

                   userProfile = userProfileRepository.Get(userProfile.Id);

                   try
                   {
                       await identityRepository.Create(userProfile, password);

                       User user = await identityRepository.GetUserByEmail(userProfile.Email);

                       var emailConfirmationToken = await identityRepository.GenerateEmailConfirmationToken(user);
                       var mailService = _mailSenderFactory.NewMailSenderService();
                       mailService.SendTokenToEmail(userProfile.Email, emailConfirmationToken, "https://localhost:7197/", user.Id);

                       return userProfile;
                   }
                   catch (Exception)
                   {
                       userProfileRepository.Remove(userProfile.Id);

                       throw;
                   }
               }
           });

        public Task<UserProfile> FullUpdate(UserProfile userProfile, string password) =>
                Task.Run(async () =>
                {
                    if (userProfile == null) throw new Exception("Dev exception. Entity can not be empty");

                    using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                    {
                        IUserProfileRepository userProfileRepository =
                            _identityRepositoriesFactory.NewUserProfileRepository(connection);

                        UserProfile existingProfile =
                            userProfileRepository.Get(userProfile.NetUid);

                        if (existingProfile == null) throw new Exception("Dev exception. Such user does not exists");

                        IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository();

                        if (!existingProfile.Email.Equals(userProfile.Email))
                        {
                            User userByEmail = await identityRepository.GetUserByEmail(userProfile.Email);

                            if (userByEmail != null) throw new Exception("User with specified email is already exists");
                        }

                        User user = await identityRepository.GetUserByUserNetId(userProfile.NetUid);

                        if (!existingProfile.Email.Equals(userProfile.Email))
                        {
                            await identityRepository.UpdateUsersEmail(user, userProfile.Email);
                            await identityRepository.UpdateUsersUserName(user, userProfile.Email);
                        }

                        if (!existingProfile.UserName.Equals(userProfile.UserName))
                        {
                            identityRepository.UpdateUsersDisplayName(user, userProfile.UserName);
                        }

                        if (!existingProfile.GrantAdministrativePermissions.Equals(userProfile.GrantAdministrativePermissions))
                        {
                            await identityRepository.ReAssignUsersRole(user, userProfile.GrantAdministrativePermissions);
                        }

                        if (!string.IsNullOrEmpty(password))
                        {
                            await identityRepository.UpdatePassword(user, password);
                        }

                        existingProfile.Email = userProfile.Email;
                        existingProfile.UserName = userProfile.UserName;
                        existingProfile.GrantAdministrativePermissions = userProfile.GrantAdministrativePermissions;

                        userProfileRepository.Update(existingProfile);

                        return userProfileRepository.Get(existingProfile.Id);
                    }
                });

        public Task<UserProfile> GetById(Guid userNetId) =>
               Task.Run(() =>
               {
                   using IDbConnection connection = _connectionFactory.NewSqlConnection();
                  
                   return _identityRepositoriesFactory.NewUserProfileRepository(connection).Get(userNetId);

               });
        public Task<List<UserProfile>> GetAll() =>
               Task.Run(async () =>
               {
                   using IDbConnection connection = _connectionFactory.NewSqlConnection();
                   IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository();

                   List<UserProfile> userProfiles = new List<UserProfile>();

                   List<User> users = await identityRepository.GetAllUsers();
                   var userProfileList = _identityRepositoriesFactory.NewUserProfileRepository(connection).GetAll();
                   foreach (var user in users)
                   {
                       var s =await identityRepository.GetRolesUser(user);
                       userProfileList.First(x => x.Email == user.Email).Role = s.First();
                   }


                   return userProfileList;

               });
        public  Task<UserProfile> GetAuthorById(Guid userNetId) =>
              Task.Run(async () =>
              {
                  using IDbConnection connection = _connectionFactory.NewSqlConnection();
                  IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository();

                  UserProfile user = _identityRepositoriesFactory.NewUserProfileRepository(connection).GetAuthorInfo(userNetId);
                  User userByEmail = await identityRepository.GetUserByEmail(user.Email);

                  var s = await identityRepository.GetRolesUser(userByEmail);
                  user.Role = s.First();
                  return user;

              });

        public Task<List<FavoriteArticle>> GetAllFavoriteArticle(Guid userProfileNetUid) =>
               Task.Run(() =>
               {
                   using IDbConnection connection = _connectionFactory.NewSqlConnection();

                   var user = _identityRepositoriesFactory.NewUserProfileRepository(connection).Get(userProfileNetUid);
                   return _identityRepositoriesFactory.NewUserProfileRepository(connection).GetAllFavoriteArticle(user.Id);
               });

        public Task<long> RemoveFavoriteArticle(Guid netUidFavoriteArticle) =>
               Task.Run(() =>
               {
                   using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                   {
                       return _identityRepositoriesFactory.NewUserProfileRepository(connection).RemoveFavoriteArticle(netUidFavoriteArticle);
                   }
               });

        public Task<FavoriteArticle> GetFavoriteArticle(Guid netUidArticle, Guid netUidUser) =>
            Task.Run(() => {

                using IDbConnection connection = _connectionFactory.NewSqlConnection();

                var user = _identityRepositoriesFactory.NewUserProfileRepository(connection).Get(netUidUser);
                var article = _mainRepositoryFactory.New(connection).GetArticle(netUidArticle);
                var favoriteArticle =_identityRepositoriesFactory.NewUserProfileRepository(connection).GetFavoriteArticle(user.Id, article.Id);
                return favoriteArticle;
            });

        public Task<long> SetFavoriteArticle(Guid netUidArticle, Guid netUidUser) =>
               Task.Run(() =>
               {
                   using IDbConnection connection = _connectionFactory.NewSqlConnection();

                   var user = _identityRepositoriesFactory.NewUserProfileRepository(connection).Get(netUidUser);
                   var article = _mainRepositoryFactory.New(connection).GetArticle(netUidArticle);
                   var identityRepository = _identityRepositoriesFactory.NewUserProfileRepository(connection);
                   if (identityRepository.GetFavoriteArticle(article.Id, user.Id)!= null)
                   {
                       throw new Exception("Favorite Article is already exists ");
                   }
                   return identityRepository.SetFavoriteArticle(article.Id, user.Id);
               });

        public Task<UserProfile> UpdateAccountInformation(UserProfile userProfile, IFormFile photoUserProfile) =>
               Task.Run(async () =>
               {
                   using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                   {
                       IUserProfileRepository userProfileRepository =
                            _identityRepositoriesFactory.NewUserProfileRepository(connection);
                       string exention = ".png";

                       UserProfile existingProfile =
                           userProfileRepository.Get(userProfile.NetUid);
                       if (photoUserProfile != null)
                       {
                           string pathLogo = Path.Combine(ArticleAdventureFolderManager.GetFilesFolderPath(), ArticleAdventureFolderManager.GetStaticImageFolder(), photoUserProfile.FileName + exention);
                           existingProfile.LinkPictureUser = Path.Combine(ArticleAdventureFolderManager.GetStaticImageFolder(), photoUserProfile.FileName + exention);
                           
                           using (var stream = new FileStream(pathLogo, FileMode.Create))
                           {
                               await photoUserProfile.CopyToAsync(stream);
                           }
                       }
                       if (existingProfile == null) throw new Exception("Dev exception. Such user does not exists");

                       IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository();

                       User user = await identityRepository.GetUserByUserNetId(userProfile.NetUid);


                       existingProfile.SurName = userProfile.SurName;
                       existingProfile.InformationAccount = userProfile.InformationAccount;
                       existingProfile.UserName = userProfile.UserName;
                       existingProfile.LinkFacebook = userProfile.LinkFacebook;
                       existingProfile.LinkTelegram = userProfile.LinkTelegram;
                       existingProfile.LinkInstagram = userProfile.LinkInstagram;
                       existingProfile.LinkTwitter = userProfile.LinkTwitter;

                       userProfileRepository.UpdateAccountInformation(existingProfile);

                       return userProfileRepository.Get(existingProfile.Id);

                   }
               });

        public Task<UserProfile> UpdateEmail(Guid userProfileNetUid, string email, string password) =>
               Task.Run(async () =>
               {
                   if (userProfileNetUid == null) throw new Exception("Dev exception. Entity can not be empty");

                   using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                   {
                       IUserProfileRepository userProfileRepository =
                            _identityRepositoriesFactory.NewUserProfileRepository(connection);

                       UserProfile existingProfile =
                           userProfileRepository.Get(userProfileNetUid);

                       if (existingProfile == null) throw new Exception("Dev exception. Such user does not exists");

                       IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository();

                       User user = await identityRepository.GetUserByUserNetId(userProfileNetUid);

                       var isPassword = await identityRepository.CheckPassword(existingProfile, password);
                       if (!isPassword) throw new Exception("The password you entered is incorrect");

                       if (!existingProfile.Email.Equals(email))
                       {
                           await identityRepository.UpdateUsersEmail(user, email);
                       }

                       existingProfile.Email = email;
                       User userСonfirmEmail = await identityRepository.GetUserByUserNetId(userProfileNetUid);
                       var emailConfirmationToken = await identityRepository.GenerateEmailConfirmationToken(userСonfirmEmail);
                       await ConforimEmail(emailConfirmationToken, userСonfirmEmail.Id);
                       userProfileRepository.Update(existingProfile);

                       return userProfileRepository.Get(existingProfile.Id);
                   }

               });

        public Task<UserProfile> UpdatePassword(Guid netUiduserProfile, string password, string oldPassword) =>
               Task.Run(async () =>
               {
                   if (netUiduserProfile == null) throw new Exception("Dev exception. Entity can not be empty");

                   using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                   {
                       IUserProfileRepository userProfileRepository =
                            _identityRepositoriesFactory.NewUserProfileRepository(connection);

                       UserProfile existingProfile =
                           userProfileRepository.Get(netUiduserProfile);

                       if (existingProfile == null) throw new Exception("Dev exception. Such user does not exists");

                       IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository();

                       User user = await identityRepository.GetUserByUserNetId(netUiduserProfile);
                       var isPassword = await identityRepository.CheckPassword(existingProfile, oldPassword);
                       if (!isPassword) throw new Exception("The password you entered is incorrect");

                       if (!string.IsNullOrEmpty(password))
                       {
                           await identityRepository.UpdatePassword(user, password);
                       }
                       return userProfileRepository.Get(existingProfile.Id);
                   }

               });

        public Task RemoveUser(Guid netUiduserProfile) =>
            Task.Run(async () =>
            {
                if (netUiduserProfile == null) throw new Exception("Dev exception. Entity can nit be empty");

                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    IUserProfileRepository userProfileRepository =
                          _identityRepositoriesFactory.NewUserProfileRepository(connection);

                    UserProfile existingProfile =
                        userProfileRepository.Get(netUiduserProfile);

                    if (existingProfile == null) throw new Exception("Dev exception. Such user does not exists");
                    IIdentityRepository identityRepository = _identityRepositoriesFactory.NewIdentityRepository();

                    userProfileRepository.Remove(existingProfile.NetUid);

                    User user = await identityRepository.GetUserByUserNetId(netUiduserProfile);
                    await identityRepository.Delete(user.NetId);
                }

            });
    }

}
 