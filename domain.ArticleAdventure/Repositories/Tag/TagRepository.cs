using Azure;
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
    internal class TagRepository : ITagRepository
    {
        private readonly IDbConnection _connection;
        public TagRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public long AddMainTag(MainTag tag)
            => _connection.Query<long>("INSERT INTO [MainTags] " +
                "([Name], [Color], [Updated] ) " +
                "VALUES " +
                "(@Name, @Color, GETUTCDATE()); " +
                "SELECT SCOPE_IDENTITY()", tag).Single();

        public long AddTag(SupTag tag)
            => _connection.Query<long>("INSERT INTO [SubTags] " +
                "([IdMainTag], [Name], [Color], [Updated] ) " +
                "VALUES " +
                "(@IdMainTag, @Name, @Color, GETUTCDATE()); " +
                "SELECT SCOPE_IDENTITY()", tag).Single();

        //public List<MainTag> AllMainTag()
        //    => _connection.Query<MainTag>("SELECT * FROM [MainTags] AS Tags " +
        //        "WHERE Tags.Deleted = 0 ").ToList();
        public List<MainTag> AllMainTag()
        {

            List<MainTag> mainTags = new List<MainTag>();

            _connection.Query<MainTag, SupTag, MainTag>("SELECT mainTag.*, subTags.* " +
                   "FROM [MainTags] AS mainTag " +
                   "LEFT JOIN [SubTags] AS subTags " +
                   "ON mainTag.Id = subTags.IdMainTag " +
                   "AND subTags.Deleted = 0 " +
                   "WHERE mainTag.Deleted = 0 ",
                (mainTag, subTag) =>
                {
                    if (mainTags.Any(c => c.Id.Equals(mainTag.Id)))
                    {
                        mainTag = mainTags.First(c => c.Id.Equals(mainTag.Id));
                    }
                    else
                    {
                        mainTags.Add(mainTag);
                    }
                    if (subTag != null)
                    {
                        mainTag.SubTags.Add(subTag);
                    }
                    return mainTag;
                }).ToList();

            return mainTags;
        }

        //"SELECT * FROM [MainTags] AS Blog " +
        //        "WHERE Blog.Deleted = 0 "

        public List<SupTag> AllTag()
        {
            throw new NotImplementedException();
        }

        public void ChangeMainTag(MainTag tag)
        => _connection.Execute("UPDATE [MainTags] " +
            "SET [Name] = @Name, [Color] = @Color, [Updated] = getutcdate() " +
            "WHERE [MainTags].NetUID = @NetUid", tag);

        public void ChangeTag(SupTag tag)
            => _connection.Execute("UPDATE [SubTags] " +
            "SET [Name] = @Name, [Color] = @Color, [Updated] = getutcdate() " +
            "WHERE [SubTags].NetUID = @NetUid", tag);

        public MainTag GetMainTag(Guid NetUidTag)
            => _connection.Query<MainTag>("SELECT * FROM [MainTags] AS Tags " +
                "WHERE Tags.Deleted = 0 " +
                "AND Tags.NetUID = @NetUid", new { NetUid = NetUidTag }).Single();

        public SupTag GetSupTag(Guid NetUidTag)
        => _connection.Query<SupTag>("SELECT * FROM [SubTags] AS Tags " +
                "WHERE Tags.Deleted = 0 " +
                "AND Tags.NetUID = @NetUid", new { NetUid = NetUidTag }).Single();

        public void RemoveMainTag(Guid NetUidTag) =>
            _connection.Execute("UPDATE [MainTags] " +
                "SET [Deleted] = 1 " +
                "WHERE [MainTags].NetUID = @NetUID",
                new { NetUID = NetUidTag });

        public void RemoveSupTag(Guid NetUidTag) =>
            _connection.Execute("UPDATE [SubTags] " +
                "SET [Deleted] = 1 " +
                "WHERE [SubTags].NetUID = @NetUID",
                new { NetUID = NetUidTag });
    }
}
