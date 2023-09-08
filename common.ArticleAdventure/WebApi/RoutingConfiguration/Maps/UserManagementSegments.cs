using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.WebApi.RoutingConfiguration.Maps
{
    public static class UserManagementSegments
    {
        public const string REQUEST_TOKEN = "token/request";

        public const string REFRESH_TOKEN = "token/refresh";

        public const string DELETE_TOKEN = "token/delete";

        public const string CREATE_USER = "create";

        public const string FULL_UPDATE_USER_PROFILE = "update";

        public const string EMAIL_CONFORMATION = "email";

        public const string GET_USER_NETUID = "get/netuid";

        public const string UPDATE_PASSWORD = "update/password";

        public const string UPDATE_EMAIL = "update/email";

        public const string UPDATE_ACCOUNT_INFORMATION = "update/account/information";

        public const string SET_FAVORITE_ARTICLE = "set/favorite/article";

        public const string GET_FAVORITE_ARTICLE = "get/favorite/article";

        public const string REMOVE_FAVORITE_ARTICLE = "remove/favorite/article";
    }
}
