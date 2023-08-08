using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domain.ArticleAdventure.Entities;
namespace service.ArticleAdventure.Services.Tag.Contracts
{
    public interface ITagService
    {
        Task<MainTag> GetMainTag(Guid netUidMainTag);
        Task<long> AddMainTag(MainTag tag);
        Task RemoveMainTag(Guid NetUidTag);
        Task ChangeMainTag(MainTag tag);
        Task<List<MainTag>> AllMainTag();
        Task<SupTag> GetSupTag(Guid netUidMainTag);
        Task<long> AddTag(SupTag tag);
        Task RemoveTag(Guid NetUidTag);
        Task ChangeSupTag(SupTag tag);
        Task<List<SupTag>> AllTag();


    }
}
