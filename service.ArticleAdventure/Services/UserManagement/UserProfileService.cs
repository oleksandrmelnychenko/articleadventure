using Azure.Core;
using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.IdentityEntities;
using domain.ArticleAdventure.Repositories.Identity.Contracts;
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


        public UserProfileService(
              IDbConnectionFactory connectionFactory,
              IIdentityRepositoriesFactory identityRepositoriesFactory,
              IMailSenderFactory mailSenderFactory)
        {
            _connectionFactory = connectionFactory;
            _identityRepositoriesFactory = identityRepositoriesFactory;
            _mailSenderFactory = mailSenderFactory;
        }
        public Task<IdentityResult> ConforimEmail(string emailConfirmationToken,string userId) => Task.Run(async () =>
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
    }

}
