﻿using domain.ArticleAdventure.DbConnectionFactory.Contracts;
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

        public Task<long> AddBlog(Blogs blog)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _blogRepositoryFactory.New(connection).AddBlog(blog);
                }
            });
                
        }

        public Task<Blogs> GetBLog(Guid netUid)
            => Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _blogRepositoryFactory.New(connection).GetBlog(netUid);
                }
            });


        public Task<List<Blogs>> GetAllBlogs()
        {
            return Task.Run(() => 
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _blogRepositoryFactory.New(connection).GetAllBlogs();
                }
            });
        }

        public Task Remove(Guid netUid) 
            => Task.Run(()=> {

                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                     _blogRepositoryFactory.New(connection).Remove(netUid);
                }
            });

        public Task Update(Blogs blogs)
          => Task.Run(() => {

              using (IDbConnection connection = _connectionFactory.NewSqlConnection())
              {
                   _blogRepositoryFactory.New(connection).Update(blogs);
              }
          });

    }
}