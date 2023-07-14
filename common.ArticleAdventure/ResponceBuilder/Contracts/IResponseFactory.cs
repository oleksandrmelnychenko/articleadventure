using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.ResponceBuilder.Contracts
{
    public interface IResponseFactory
    {
        IWebResponse GetSuccessResponse();

        IWebResponse GetErrorResponse();
    }
}
