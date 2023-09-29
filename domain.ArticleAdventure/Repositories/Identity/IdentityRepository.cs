using common.ArticleAdventure.IdentityConfiguration;
using Dapper;
using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.IdentityEntities;
using domain.ArticleAdventure.Repositories.Identity.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Identity
{
    public sealed class IdentityRepository : IIdentityRepository
    {
        private RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IDbConnectionFactory _connectionFactory;

        public IdentityRepository(
            IDbConnectionFactory connectionFactory,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _connectionFactory = connectionFactory;
            _roleManager = roleManager;
        }

        public async Task<Tuple<ClaimsIdentity, User>> AuthAndGetClaimsIdentity(string email, string password)
        {
            //List<string> strings = new List<string>();

            User user = await _userManager.FindByEmailAsync(email);
            //var userRoles = await _userManager.GetRolesAsync(user);
            //// получаем все роли
            //List<IdentityRole> allRoles = _roleManager.Roles.ToList();
            //strings.Add(allRoles.Last().Name);
            // получаем список ролей, которые были добавлены
            //await _userManager.AddToRolesAsync(user, strings);
            if (user == null) throw new Exception("There is no account for the email you entered");

            var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (!emailConfirmed) throw new Exception("Mail is not confirmed");


            if (!await _userManager.CheckPasswordAsync(user, password)) throw new Exception("The password you entered is incorrect");

            IList<string> roles = await _userManager.GetRolesAsync(user);
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);

            claims.Add(new Claim("role", roles.FirstOrDefault() ?? string.Empty));
            claims.Add(new Claim("UserName", user.DisplayName));
            claims.Add(new Claim("Email", user.Email));

            return new Tuple<ClaimsIdentity, User>(new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"), claims), user);
        }

        public async Task<Tuple<ClaimsIdentity, User>> AuthAndGetClaimsIdentity(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);

            if (user == null) return null;

            IList<string> roles = await _userManager.GetRolesAsync(user);
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);

            claims.Add(new Claim("role", roles.FirstOrDefault() ?? string.Empty));
            claims.Add(new Claim("UserName", user.DisplayName));
            claims.Add(new Claim("Email", user.Email));

            return new Tuple<ClaimsIdentity, User>(new ClaimsIdentity(new GenericIdentity(user.DisplayName, "Token"), claims), user);
        }

        public async Task<IEnumerable<Claim>> AuthAndGetClaimsIdentity(User user)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);

            claims.Add(new Claim("role", roles.FirstOrDefault() ?? string.Empty));
            claims.Add(new Claim("UserName", user.DisplayName));
            claims.Add(new Claim("Email", user.Email));

            return new ClaimsIdentity(new GenericIdentity(user.DisplayName, "Token"), claims).Claims;
        }

        public async Task Create(UserProfile userProfile, string password)
        {
            User user = await _userManager.FindByEmailAsync(userProfile.Email);

            if (user != null) throw new Exception("User with such email is already exists");

            user = new User
            {
                Email = userProfile.Email,
                UserName = userProfile.Email,
                DisplayName = userProfile.UserName,
                NetId = userProfile.NetUid
            };

            IdentityResult createResult = await _userManager.CreateAsync(user, password);

            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, userProfile.GrantAdministrativePermissions ? IdentityRoles.Administrator : IdentityRoles.User);

                await _userManager.AddClaimAsync(user, new Claim("NetId", userProfile.NetUid.ToString()));
            }
            else
            {
                throw new Exception(createResult.Errors.FirstOrDefault()?.Description ?? string.Empty);
            }
        }

        public async Task UpdatePassword(Guid userNetId, string newPassword)
        {
            IList<User> users = await _userManager.GetUsersForClaimAsync(new Claim("NetId", userNetId.ToString()));

            if (!users.Any()) throw new Exception("Such user does not exist");

            User user = users.First();

            IdentityResult resetPasswordResult = await _userManager
                .ResetPasswordAsync(
                    user,
                    await _userManager.GeneratePasswordResetTokenAsync(user),
                    newPassword
                );

            if (resetPasswordResult.Succeeded) return;

            throw new Exception(resetPasswordResult.Errors.FirstOrDefault()?.Description ?? string.Empty);
        }

        public async Task UpdateUser(User user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdatePassword(User user, string newPassword)
        {
            IdentityResult resetPasswordResult = await _userManager
                .ResetPasswordAsync(
                    user,
                    await _userManager.GeneratePasswordResetTokenAsync(user),
                    newPassword
                );

            if (resetPasswordResult.Succeeded) return;

            throw new Exception(resetPasswordResult.Errors.FirstOrDefault()?.Description ?? string.Empty);
        }

        public async Task<string> GetUserIdByUserNetId(Guid userNetId)
        {
            IList<User> users = await _userManager.GetUsersForClaimAsync(new Claim("NetId", userNetId.ToString()));

            return users.FirstOrDefault()?.Id ?? string.Empty;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await _userManager.FindByIdAsync("2");
            //if (user != null)
            //{
            //    // получем список ролей пользователя
            //    var userRoles = await _userManager.GetRolesAsync(user);
            //    var allRoles = _roleManager.Roles.ToList();
            //    ChangeRoleViewModel model = new ChangeRoleViewModel
            //    {
            //        UserId = user.Id,
            //        UserEmail = user.Email,
            //        UserRoles = userRoles,
            //        AllRoles = allRoles
            //    };
            //    return View(model);
            //}


            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<List<User>> GetAllUsers()
        {
           return _userManager.Users.ToList();
        }

        public async Task<IList<string>> GetRolesUser(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<string> GenerateEmailConfirmationToken(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<User> GetUserByUserName(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
        public async Task<User> FindByIdAsync(string userid)
        {
            return await _userManager.FindByIdAsync(userid);
        }
        public async Task<User> GetUserByUserNetId(Guid userNetId)
        {
            IList<User> users = await _userManager.GetUsersForClaimAsync(new Claim("NetId", userNetId.ToString()));
            return users.FirstOrDefault();
        }



        public async Task UpdateUsersEmail(User user, string email)
        {
            IdentityResult updateResult = await _userManager.SetEmailAsync(user, email);

            if (updateResult.Succeeded) return;

            throw new Exception(updateResult.Errors.FirstOrDefault()?.Description ?? string.Empty);
        }

        public async Task UpdateUsersUserName(User user, string username)
        {
            IdentityResult updateResult = await _userManager.SetUserNameAsync(user, username);

            if (updateResult.Succeeded) return;

            throw new Exception(updateResult.Errors.FirstOrDefault()?.Description ?? string.Empty);
        }

        public void UpdateUsersDisplayName(User user, string username)
        {
            using IDbConnection connection = _connectionFactory.NewSqlConnection();
            connection.Execute(
                "UPDATE [AspNetUsers] " +
                "SET DisplayName = @DisplayName " +
                "WHERE Id = @Uid",
                new { Uid = user.Id, DisplayName = username });
            user.DisplayName = username;
        }


        public async Task ReAssignUsersRole(User user, bool grantAdministrativePermissions)
        {
            await _userManager.RemoveFromRolesAsync(
                user,
                await _userManager.GetRolesAsync(user)
            );

            await _userManager.AddToRoleAsync(user, grantAdministrativePermissions ? IdentityRoles.Administrator : IdentityRoles.User);
        }

        public async Task ResetPassword(Guid userNetId, Guid resetToken, string newPassword)
        {
            IList<User> users = await _userManager.GetUsersForClaimAsync(new Claim("NetId", userNetId.ToString()));

            if (!users.Any()) throw new Exception("Such user does not exist");

            User user = users.First();

            if (user.ResetPasswordToken != resetToken) throw new Exception("Reset token is invalid");

            IdentityResult resetPasswordResult = await _userManager
                .ResetPasswordAsync(
                    user,
                    await _userManager.GeneratePasswordResetTokenAsync(user),
                    newPassword
                );

            if (resetPasswordResult.Succeeded)
            {
                user.ResetPasswordToken = null;

                await _userManager.UpdateAsync(user);

                return;
            }

            throw new Exception(resetPasswordResult.Errors.FirstOrDefault()?.Description ?? string.Empty);
        }

        public async Task<bool> CheckPassword(UserProfile userProfile, string password)
        {
            User user = await _userManager.FindByEmailAsync(userProfile.Email);

            if (await _userManager.CheckPasswordAsync(user, password))
            {
                return true;
            }

            return false;


        }

        public async Task Delete(Guid userNetId)
        {
            IList<User> users = await _userManager.GetUsersForClaimAsync(new Claim("NetId", userNetId.ToString()));

            if (!users.Any()) return;

            User user = users.First();

            await _userManager.DeleteAsync(user);
        }
    }
}
