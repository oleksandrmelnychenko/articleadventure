using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using service.ArticleAdventure.Services.Blog.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public Task<long> AddBlog()
        {
            throw new NotImplementedException();
        }

        public Task<long> AddBlog(Blogs blog)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    long foo = _blogRepositoryFactory.New(connection).AddBlog(blog);
                    return foo;
                }
            });
                
        }

       
    }
}
