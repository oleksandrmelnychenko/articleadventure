using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.WebApi.RoutingConfiguration.Maps
{
    public static class ArticleSegments
    {
        public const string ADD_ARTICLE = "add";
        public const string REMOVE_ARTICLE = "remove";
        public const string EDIT = "edit";
        public const string ALL_ARTICLE = "all";
        public const string ALL_ARTICLE_FILTERED_DATE_TIME = "all/filtered/datetime";
        public const string GET_ALL_ARTICLE_FILTER_SUP_TAGS = "get/all/suptag";
        public const string UPDATE = "update";
        public const string GET_SUP_ARTICLE = "get/suparticle";
        public const string GET_ALL_BLOGS = "get/all";
        public const string GET_ARTICLE = "get";
        public const string GET_ALL_ARTICLE_USER = "get/user/articles";
        public const string GET_USER_ALL_STRIPE_PAYMENTS = "get/user/stripepayments";
        public const string GET_USER_ARTICLE = "get/user/article";
    }
}
