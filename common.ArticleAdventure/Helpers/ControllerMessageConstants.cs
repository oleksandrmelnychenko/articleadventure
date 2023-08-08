using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.Helpers
{
    public  class ControllerMessageConstants
    {
        public class TagMessage
        {
            public const string AddTag = "Status add successfully.";
            public const string RemoveTag = "Status remove successfully.";
            public const string UpdateTag = "Status update successfully.";
            public const string AllTag = "Get All Article successfully.";
            public const string GetTag = "Successfully pulled data.";
        }
        public class ArticleMessage
        {
            public const string AddArticle = "Status add successfully";
            public const string RemoveArticle = "Status remove successfully";
            public const string UpdateArticle = "Status update successfully";
            public const string AllArticle = "Get All Article successfully";
        }
    }
}
