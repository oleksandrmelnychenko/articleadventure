﻿using System;
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
    }
}