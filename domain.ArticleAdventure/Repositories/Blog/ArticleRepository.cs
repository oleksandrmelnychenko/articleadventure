using Dapper;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using System.Data;

namespace domain.ArticleAdventure.Repositories.Blog
{
    public class ArticleRepository: IBlogRepository
    {
        private readonly IDbConnection _connection;
        public ArticleRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public long AddArticle(AuthorArticle blog) 
            => _connection.Query<long>("INSERT INTO [AuthorArticle] " +
            "([MainArticleId], [Title], [Description], [Body] ,[Image] ,[ImageUrl] ,[WebImageUrl] ,[EditorValue] ,[MetaKeywords] " +
            ",[MetaDescription] ,[Price] ,[Url] ,[Updated] ) " +
            "VALUES " +
            "(@MainArticleId, @Title, @Description, @Body, @Image, @ImageUrl, @WebImageUrl" +
            ", @EditorValue, @MetaKeywords, @MetaDescription, @Price, @Url, GETUTCDATE());" +
            "SELECT SCOPE_IDENTITY()", blog
            ).Single();

        public List<AuthorArticle> GetAllArticles() 
            => _connection.Query<AuthorArticle>("SELECT * FROM [Blogs] AS Blog " +
                "WHERE Blog.Deleted = 0 ").ToList();

        public AuthorArticle GetArticle(Guid netUid) => _connection.Query<AuthorArticle>("SELECT * FROM [Blogs] AS Blog " +
                "WHERE Blog.NetUid = @NetUid"
            ,new { NetUid = netUid}).Single();

        public void Remove(Guid netUid) =>
            _connection.Execute("UPDATE [ArticleAdventure].[dbo].[AuthorArticle] " +
                "SET [Deleted] = 1 " +
                "WHERE [ArticleAdventure].[dbo].[AuthorArticle].NetUID = @NetUID",
                new { NetUID = netUid }
                );
        public void Update(AuthorArticle blog) =>
            _connection.Execute("Update [AuthorArticle] " +
                "SET [Title] = @Title, [Description] = @Description, [Body] = @Body ,[Image] = @Image " +
                ",[ImageUrl] = @ImageUrl ,[WebImageUrl] = @WebImageUrl ,[EditorValue] = @EditorValue ,[MetaKeywords] = @MetaKeywords " +
                ",[MetaDescription] = @MetaDescription ,[Price] = @Price ,[Url] = @Url ,[Updated] = GETUTCDATE() " +
                $"WHERE [AuthorArticle].[NetUid] = @NetUid ",
                blog);
    }
}
