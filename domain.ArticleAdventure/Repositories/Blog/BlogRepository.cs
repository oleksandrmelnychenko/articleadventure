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

        public long AddBlog(Blogs blog) 
            => _connection.Query<long>("INSERT INTO [Blogs] " +
            "([Title], [Description], [Body] ,[Image] ,[ImageUrl] ,[WebImageUrl] ,[EditorValue] ,[MetaKeywords] " +
            ",[MetaDescription] ,[Url] ,[Updated] ) " +
            "VALUES " +
            "(@Title, @Description, @Body, @Image, @ImageUrl, @WebImageUrl" +
            ", @EditorValue, @MetaKeywords, @MetaDescription, @Url, GETUTCDATE());" +
            "SELECT SCOPE_IDENTITY()", blog
            ).Single();

        public List<Blogs> GetAllBlogs() 
            => _connection.Query<Blogs>("SELECT * FROM [Blogs] AS Blog " +
                "WHERE Blog.Deleted = 0 ").ToList();
    }
}
