﻿using common.ArticleAdventure.WebApi.RoutingConfiguration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    public class MainArticle:EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ImageUrl { get; set; }
        public string WebImageUrl { get; set; }
        public List<AuthorArticle> Articles { get; set; }
        public string InfromationArticle { get; set; }
        public  List<MainArticleTags> ArticleTags { get; set; }
        public double Price { get; set; }
        public long UserId { get; set; }
        public UserProfile UserProfile { get; set; }

        public MainArticle()
        {
            UserProfile = new UserProfile();
            Articles = new List<AuthorArticle>();
            ArticleTags = new List<MainArticleTags>();
        }
    }
}
