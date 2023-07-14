using common.ArticleAdventure.ResponceBuilder.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using NLog;

namespace common.ArticleAdventure.WebApi
{
    public abstract class WebApiControllerBase : Controller
    {
        private readonly IResponseFactory _responseFactory;

        /// <summary>
        ///     NLog logger
        /// </summary>
        protected Logger Logger { get; }

        /// <summary>
        ///     ctor().
        /// </summary>
        protected WebApiControllerBase(IResponseFactory responseFactory)
        {
            Logger = LogManager.GetCurrentClassLogger();

            _responseFactory = responseFactory;
        }

        protected IWebResponse SuccessResponseBody(object body, string message = "")
        {
            IWebResponse response = _responseFactory.GetSuccessResponse();

            response.Body = body;
            response.StatusCode = HttpStatusCode.OK;
            response.Message = message;

            return response;
        }

        protected IWebResponse ErrorResponseBody(string message, HttpStatusCode statusCode)
        {
            IWebResponse response = _responseFactory.GetErrorResponse();

            response.Message = message;
            response.StatusCode = statusCode;

            return response;
        }
    }
}
