﻿using domain.ArticleAdventure.FilterBuilders.MainArticles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.FilterBuilders
{
    public interface IFilterBuilderFactory
    {
        IMainArticleFilterBuilder NewMainArticleFilterBuilder();

    }
}
