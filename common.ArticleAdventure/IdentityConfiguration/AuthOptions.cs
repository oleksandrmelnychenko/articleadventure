using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.IdentityConfiguration
{
    public static class AuthOptions
    {
        public const string ISSUER = "ArticleAdventure";

        public const string AUDIENCE_LOCAL = "https://localhost:7192/";
        public const string AUDIENCE_REMOTE = "https://localhost:7192/";

        public const string DEFAULT_PASSWORD_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public const string KEY = "business_secret_key_dev";

        public const int LIFETIME = 1500;

        public const int REFRESH_LIFETIME = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
