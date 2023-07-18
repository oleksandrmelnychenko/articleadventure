using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using service.ArticleAdventure.Services.Blog.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services.Blog
{
    public class BlogService : BaseService , IBlogService
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IBlogRepositoryFactory _blogRepositoryFactory;
        public BlogService(IDbConnectionFactory connectionFactory,
            IBlogRepositoryFactory blogRepositoryFactory) : base(connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _blogRepositoryFactory = blogRepositoryFactory;
        }
        public Task<long> GetTestBlog()
        {
            return Task.Run(() => {
                List<Tag> tags = new List<Tag>();
                Tag tag2 = new Tag
                {
                    Id = 2,
                    Name = "Tag 2",
                    IsSelected = false,
                    Color = "#00FF00"
                };
                tags.Add(tag2);
                // Создаем экземпляр класса Blog
                Blogs blog = new Blogs
                {
                    Id = 1,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    NetUid = Guid.NewGuid(),
                    Title = "My Blog Post",
                    Description = "This is a blog post",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    Image = "4",
                    ImageUrl = "1",
                    WebImageUrl = "2",
                    //BlogTags = tags,
                    EditorValue = "3",
                    MetaKeywords = "keyword1, keyword2",
                    MetaDescription = "This is the meta description",
                    Url = "https://example.com/blog/my-blog-post"
                };
                //return blog;
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    var foo = _blogRepositoryFactory.New(connection).GetTestBlog(blog);
                    return foo;
                }
            });
        }
    }
}
