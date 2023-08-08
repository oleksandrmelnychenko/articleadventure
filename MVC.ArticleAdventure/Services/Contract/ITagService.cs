using domain.ArticleAdventure.Entities;

namespace MVC.ArticleAdventure.Services.Contract
{
    public interface ITagService
    {
        Task<List<MainTag>> GetAllTags();
        Task AddMainTag(MainTag blogs);
        Task<MainTag> GetMainTag(Guid NetUidMainTag);
        Task ChangeMainTag(MainTag mainTag);
        Task RemoveMainTag(Guid NetUidMainTag);

        Task AddSupTag(SupTag blogs);
        Task<SupTag> GetSupTag(Guid NetUidMainTag);
        Task RemoveSupTag(Guid NetUidMainTag);
        Task ChangeSupTag(SupTag mainTag);
    }
}
