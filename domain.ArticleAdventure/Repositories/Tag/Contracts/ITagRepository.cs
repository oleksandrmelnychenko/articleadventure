using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Tag.Contracts
{
    public interface ITagRepository
    {
        MainTag GetMainTag(Guid NetUidTag);
        long AddMainTag(MainTag tag);
        void RemoveMainTag(Guid NetUidTag);
        void ChangeMainTag(MainTag tag);
        List<MainTag> AllMainTag();
        SupTag GetSupTag(Guid NetUidTag);
        long AddTag(SupTag tag);
        void RemoveSupTag(Guid NetUidTag);
        void ChangeTag(SupTag tag);
        List<SupTag> AllTag();
    }
}
