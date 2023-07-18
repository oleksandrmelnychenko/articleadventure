using Dapper;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using System.Data;

namespace domain.ArticleAdventure.Repositories.Blog
{
    public class BlogRepository: IBlogRepository
    {
        private readonly IDbConnection _connection;
        public BlogRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public long GetTestBlog(Blogs blog)
        {
            return _connection.Query<long>("INSERT INTO [Blogs] " +
            "([Title], [Description], [Body] ,[Image] ,[ImageUrl] ,[WebImageUrl] ,[EditorValue] ,[MetaKeywords] " +
            ",[MetaDescription] ,[Url] ,[NetUid] ,[Created] ,[Deleted] ,[Updated]) " +
            "VALUES " +
            "(@Title, @Description, @Body, @Image, @ImageUrl, @WebImageUrl" +
            ", @EditorValue, @MetaKeywords, @MetaDescription, @Url, @NetUid, @Created, @Deleted , GETUTCDATE()); " +
            "SELECT SCOPE_IDENTITY()", blog
            ).Single();
            //Tag tag1 = new Tag
            //{
            //    Id = 1,
            //    Name = "Tag 1",
            //    IsSelected = true,
            //    Color = "#FF0000"
            //};



            //// Создаем список BlogTags и добавляем в него теги
            //List<Tag> blogTags = new List<Tag>();
            //blogTags.Add(tag1);
            //blogTags.Add(tag2);
            //Tag tag2 = new Tag
            //{
            //    Id = 2,
            //    Name = "Tag 2",
            //    IsSelected = false,
            //    Color = "#00FF00"
            //};
            //// Создаем экземпляр класса Blog
            //Entities.Blogs blog = new Entities.Blogs
            //{
            //    Id = 1,
            //    Title = "My Blog Post",
            //    Description = "This is a blog post",
            //    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            //    Image = null,
            //    ImageUrl = "",
            //    WebImageUrl = "",
            //    BlogTags = blogTags,
            //    EditorValue = "",
            //    MetaKeywords = "keyword1, keyword2",
            //    MetaDescription = "This is the meta description",
            //    Url = "https://example.com/blog/my-blog-post"
            //};
            //return blog;
        }
    }
}
