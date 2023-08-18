using Dapper;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.Repositories.Tag.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Tag
{
    public class MainArticleTagsService:IMainArticleTagsService
    {
        private readonly IDbConnection _connection;
        public MainArticleTagsService(IDbConnection connection)
        {
            _connection = connection;
        }

        public long AddMainTag(MainArticleTags tag) 
            => _connection.Query<long>("INSERT INTO [ArticleTags] " +
                "([MainArticleId], [SupTagId], [Updated] ) " +
                "VALUES " +
                "(@MainArticleId, @SupTagId, GETUTCDATE()); " +
                "SELECT SCOPE_IDENTITY()", tag).Single();

        public List<MainArticleTags> AllMainTag()
        {
            throw new NotImplementedException();
        }

        public void UpdateMainTag(MainArticleTags tag)
        {
            throw new NotImplementedException();
        }

        public MainTag GetMainTag(Guid NetUidTag)
        {
            throw new NotImplementedException();
        }
       
        public void RemoveMainTag(long mainArticleId)
            => _connection.Execute("DELETE FROM [ArticleAdventure].[dbo].[ArticleTags] " +
                "WHERE MainArticleId = @MainArticleId "
                , new { MainArticleId = mainArticleId }
                );
    }
}
