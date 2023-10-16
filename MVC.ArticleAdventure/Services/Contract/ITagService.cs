using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface ITagService
    {
        Task<ExecutionResult<List<MainTag>>> GetAllTags();
        Task AddMainTag(MainTag blogs, string tokenAdmin);
        Task<MainTag> GetMainTag(Guid NetUidMainTag);
        Task ChangeMainTag(MainTag mainTag,string tokenAdmin);
        Task RemoveMainTag(Guid NetUidMainTag, string tokenAdmin);
        Task AddSupTag(SupTag blogs, string tokenAdmin);
        Task<SupTag> GetSupTag(Guid NetUidMainTag);
        Task RemoveSupTag(Guid NetUidMainTag, string tokenAdmin);
        Task ChangeSupTag(SupTag mainTag, string tokenAdmin);
    }
}
