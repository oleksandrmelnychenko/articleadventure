using common.ArticleAdventure.IdentityConfiguration;
using domain.ArticleAdventure.EntityHelpers.Identity;
using domain.ArticleAdventure.IdentityEntities;
using domain.ArticleAdventure.Repositories.Identity.Contracts;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Helpers
{
    public static class TokenGenerator
    {
        public static CompleteAccessToken GenerateToken(
            IUserTokenRepository userTokensRepository,
            IEnumerable<Claim> claims,
            User user,
            bool extendRefreshTokenLifetime = false)
        {
            DateTime now = DateTime.UtcNow;

            JwtSecurityToken jwt = new JwtSecurityToken(
                AuthOptions.ISSUER,
                AuthOptions.AUDIENCE_LOCAL,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            RefreshToken newRefreshToken = new RefreshToken
            {
                UserId = user.Id,
                ExpireAt = DateTime.UtcNow.AddDays(extendRefreshTokenLifetime ? 30 : AuthOptions.REFRESH_LIFETIME)
            };

            string encryptedRefreshToken = AesManager.Encrypt(JsonConvert.SerializeObject(newRefreshToken));

            UserToken userToken = userTokensRepository.GetByUserIdIfExists(user.Id);

            if (userToken == null)
            {
                userToken = new UserToken
                {
                    Token = encryptedRefreshToken,
                    UserId = user.Id
                };

                userTokensRepository.Add(userToken);
            }
            else
            {
                userToken.Token = encryptedRefreshToken;

                userTokensRepository.Update(userToken);
            }

            return new CompleteAccessToken
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                RefreshToken = encryptedRefreshToken
            };
        }

        public static CompleteAccessToken GenerateToken(
            IUserTokenRepository userTokensRepository,
            IEnumerable<Claim> claims,
            User user,
            int refreshTokenValidMinutesCount)
        {
            DateTime now = DateTime.UtcNow;

            JwtSecurityToken jwt = new JwtSecurityToken(
                AuthOptions.ISSUER,
                AuthOptions.AUDIENCE_LOCAL,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            RefreshToken newRefreshToken = new RefreshToken
            {
                UserId = user.Id,
                ExpireAt = DateTime.UtcNow.AddMinutes(refreshTokenValidMinutesCount)
            };

            string encryptedRefreshToken = AesManager.Encrypt(JsonConvert.SerializeObject(newRefreshToken));

            UserToken userToken = userTokensRepository.GetByUserIdIfExists(user.Id);

            if (userToken == null)
            {
                userToken = new UserToken
                {
                    Token = encryptedRefreshToken,
                    UserId = user.Id
                };

                userTokensRepository.Add(userToken);
            }
            else
            {
                userToken.Token = encryptedRefreshToken;

                userTokensRepository.Update(userToken);
            }

            return new CompleteAccessToken
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                RefreshToken = encryptedRefreshToken
            };
        }
    }
}
