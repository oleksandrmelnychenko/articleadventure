﻿namespace domain.ArticleAdventure.Entities
{
    public class Blogs: EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string ImageUrl { get; set; }
        public string WebImageUrl { get; set; }
        public List<Tag> BlogTags { get; set; }
        public string EditorValue { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string Url { get; set; }
    }
}
