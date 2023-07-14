using common.ArticleAdventure.ResponceBuilder.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.ResponceBuilder
{
    public class ResponseFactory: IResponseFactory
    {
        public IWebResponse GetSuccessResponse()
        {
            return new SuccessResponse();
        }

        public IWebResponse GetErrorResponse()
        {
            return new ErrorResponse();
        }
    }
}
