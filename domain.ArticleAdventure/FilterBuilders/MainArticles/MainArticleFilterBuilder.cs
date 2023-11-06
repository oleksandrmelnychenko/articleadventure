using domain.ArticleAdventure.EntityHelpers.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.FilterBuilders.MainArticles
{
    public class MainArticleFilterBuilder : IMainArticleFilterBuilder
    {
        public string Build(MainArticleFilter filter)
        {
            string allMainArticle = string.Empty;

            if (filter == null)
                return string.Empty;

            if (filter.MainArticleId != null && filter.MainArticleId?.Count != 0)
            {
                filter.MainArticleId.ForEach(c =>
                {
                    if (allMainArticle == string.Empty)
                    {
                        allMainArticle += $@" articleTags.MainArticleId = {c} ";
                    }
                    else
                    {
                        allMainArticle += $@" OR articleTags.MainArticleId = {c} ";
                    }
                });
                return $@" AND ({allMainArticle})";

            }

            if(filter.SupTagsId != null && filter.SupTagsId?.Count != 0)
            {
                filter.SupTagsId.ForEach(c =>
                {
                    if (allMainArticle == string.Empty)
                    {
                        allMainArticle += $@" [ArticleTags].[SupTagId] = {c} ";
                    }
                    else
                    {
                        allMainArticle += $@" OR [ArticleTags].[SupTagId] = {c} ";
                    }
                });
                return $@" WHERE ({allMainArticle})";
            }
           
                return allMainArticle;


        }
        private void CheckForAndOperator(StringBuilder queryBody)
        {
            queryBody.Append($@" AND ");
        }
    }
}
