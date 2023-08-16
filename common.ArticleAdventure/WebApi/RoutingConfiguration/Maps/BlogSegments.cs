﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.WebApi.RoutingConfiguration.Maps
{
    public static class ArticleSegments
    {
        public const string ADD = "add";
        public const string REMOVE_ARTICLE = "remove";
        public const string EDIT = "edit";
        public const string ALL_ARTICLE = "all";
        public const string UPDATE = "update";
        public const string GET_ALL_BLOGS = "get/all";
        public const string GET_ARTICLE = "get";
    }
}
