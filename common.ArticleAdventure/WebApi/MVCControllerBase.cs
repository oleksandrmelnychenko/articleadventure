using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.WebApi
{
    public class MVCControllerBase:Controller
    {
        [NonAction]
        protected async Task SetErrorMessage(string ErrorMessage)
        {
            ViewBag.ErrorMessage = ErrorMessage;
        }
        [NonAction]
        protected async Task SetSuccessMessage(string SuccessMessage)
        {
            ViewBag.SuccessMessage = SuccessMessage;
        }
    }
}
