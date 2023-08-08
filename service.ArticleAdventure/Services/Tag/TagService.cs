using Azure;
using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Tag.Contracts;
using service.ArticleAdventure.Services.Tag.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services.Tag
{
    public class TagService : BaseService, ITagService
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ITagRepositoryFactory _tagRepositoryFactory;

        public TagService(IDbConnectionFactory connectionFactory, ITagRepositoryFactory tagRepositoryFactory) : base(connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _tagRepositoryFactory = tagRepositoryFactory;
        }

        public Task<long> AddMainTag(MainTag tag)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _tagRepositoryFactory.New(connection).AddMainTag(tag);
                }
            });

        }

        public Task<long> AddTag(SupTag tag)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _tagRepositoryFactory.New(connection).AddTag(tag);
                }
            });
        }

        public Task<List<MainTag>> AllMainTag()
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _tagRepositoryFactory.New(connection).AllMainTag();
                }
            });
        }

        public Task<List<SupTag>> AllTag()
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _tagRepositoryFactory.New(connection).AllTag();
                }
            });
        }

        public Task ChangeMainTag(MainTag tag)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    _tagRepositoryFactory.New(connection).ChangeMainTag(tag);
                }
            });
        }

        public Task ChangeSupTag(SupTag tag)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    _tagRepositoryFactory.New(connection).ChangeTag(tag);
                }
            });
        }

        public Task<MainTag> GetMainTag(Guid netUidMainTag)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                   return _tagRepositoryFactory.New(connection).GetMainTag(netUidMainTag);
                }
            });
        }

        public Task<SupTag> GetSupTag(Guid netUidSupTag)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    return _tagRepositoryFactory.New(connection).GetSupTag(netUidSupTag);
                }
            });
        }

        public Task RemoveMainTag(Guid NetUidMainTag)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    _tagRepositoryFactory.New(connection).RemoveMainTag(NetUidMainTag);
                }
            });
        }

        public Task RemoveTag(Guid NetUidSubTag)
        {
            return Task.Run(() =>
            {
                using (IDbConnection connection = _connectionFactory.NewSqlConnection())
                {
                    _tagRepositoryFactory.New(connection).RemoveSupTag(NetUidSubTag);
                }
            });
        }
    }
}
