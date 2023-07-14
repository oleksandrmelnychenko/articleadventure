using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Blog
{
    public class BlogRepository: IBlogRepository
    {
        private readonly IDbConnection _connection;
        public BlogRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public Blogs GetTestBlog(int id)
        {

            Tag tag1 = new Tag
            {
                Id = 1,
                Name = "Tag 1",
                IsSelected = true,
                Color = "#FF0000"
            };

            Tag tag2 = new Tag
            {
                Id = 2,
                Name = "Tag 2",
                IsSelected = false,
                Color = "#00FF00"
            };

            // Создаем список BlogTags и добавляем в него теги
            List<Tag> blogTags = new List<Tag>();
            blogTags.Add(tag1);
            blogTags.Add(tag2);

            // Создаем экземпляр класса Blog
            Entities.Blogs blog = new Entities.Blogs
            {
                Id = 1,
                Title = "My Blog Post",
                Description = "This is a blog post",
                Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Image = null,
                ImageUrl = "",
                WebImageUrl = "",
                BlogTags = blogTags,
                EditorValue = "",
                MetaKeywords = "keyword1, keyword2",
                MetaDescription = "This is the meta description",
                Url = "https://example.com/blog/my-blog-post"
            };
            return blog;
        }
    }
}
