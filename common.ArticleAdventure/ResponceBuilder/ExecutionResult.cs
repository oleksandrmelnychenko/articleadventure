using common.ArticleAdventure.ResponceBuilder.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.ResponceBuilder
{
    public class ExecutionResult<T>
    {
        public IError Error;
        public bool IsSuccess => Error == null;
        public T Data { get; set; }
    }
}
