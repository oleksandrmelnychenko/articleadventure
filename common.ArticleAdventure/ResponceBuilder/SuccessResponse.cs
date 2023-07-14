using common.ArticleAdventure.ResponceBuilder.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.ResponceBuilder
{
    public class SuccessResponse: IWebResponse
    {
        public object Body { get; set; }

        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
