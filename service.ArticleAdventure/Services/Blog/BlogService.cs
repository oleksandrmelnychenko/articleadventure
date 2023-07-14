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
        public Task<Blogs> GetTestBlog()
        {
            return Task.Run(() => {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    var foo = _blogRepositoryFactory.New(connection).GetTestBlog(1);
                    return foo;
                }
            });
        }
    }
}
