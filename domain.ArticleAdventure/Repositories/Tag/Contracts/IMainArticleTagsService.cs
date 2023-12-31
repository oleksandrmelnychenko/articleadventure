﻿using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Tag.Contracts
{
    public interface IMainArticleTagsService
    {
        MainTag GetMainTag(Guid NetUidTag);
        long AddMainTag(MainArticleTags tag);
        void RemoveMainTag(long NetUidTag);
        void UpdateMainTag(MainArticleTags tag);
        List<MainArticleTags> AllMainTag();
    }
}
