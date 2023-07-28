﻿using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Blog.Contracts
{
    public interface IUserProfileRepository
    {
        long Add(UserProfile userProfile);

        UserProfile Get(long id);

        UserProfile Get(string userName, string email);

        UserProfile Get(string email);

        UserProfile Get(Guid netId);

        IEnumerable<UserProfile> GetAllFiltered(string value, int limit, int offset);

        void Update(UserProfile userProfile);

        void Remove(long id);

        void Remove(Guid netId);
    }
}