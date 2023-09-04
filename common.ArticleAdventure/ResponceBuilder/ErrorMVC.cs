using common.ArticleAdventure.ResponceBuilder.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.ResponceBuilder
{
    public class ErrorMVC : IError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
