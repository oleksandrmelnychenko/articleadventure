using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.ResponceBuilder.Contracts
{
    public interface IError
    {
        string Message { get; set; }
        HttpStatusCode StatusCode { get; set; }
    }
}
