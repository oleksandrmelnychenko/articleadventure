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

        public Blogs GetBlog(Guid netUid) => _connection.Query<Blogs>("SELECT * FROM [Blogs] AS Blog " +
                "WHERE Blog.NetUid = @NetUid"
            ,new { NetUid = netUid}).Single();

        public void Remove(Guid netUid) =>
            _connection.Execute("UPDATE [Blogs]" +
                "SET [Deleted] = 1 " +
                "WHERE NetUid = @NetUid ", new { NetUid = netUid }
                );
        public void Update(Blogs blog) =>
            _connection.Execute("Update [Blogs] " +
                "SET [Title] = @Title, [Description] = @Description, [Body] = @Body ,[Image] = @Image " +
                ",[ImageUrl] = @ImageUrl ,[WebImageUrl] = @WebImageUrl ,[EditorValue] = @EditorValue ,[MetaKeywords] = @MetaKeywords " +
                ",[MetaDescription] = @MetaDescription ,[Url] = @Url ,[Updated] = GETUTCDATE() " +
                $"WHERE [Blogs].[NetUid] = @NetUid ",
                blog);
    }
}
