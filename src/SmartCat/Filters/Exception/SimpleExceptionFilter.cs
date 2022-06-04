using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Filter.Model;

namespace SmartCat.Filter.Exception
{
    public class SimpleExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            ExecuteException(context);
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            ExecuteException(context);
        }


        private void ExecuteException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                var result = new DefaultJsonResult()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = context.Exception.Message,
                };

                if (context.Exception is SmartCatException exception)
                {
                    result.StatusCode = exception.StatusCode;
                }

                context.Result = new JsonResult(result);
                context.HttpContext.Response.StatusCode = result.StatusCode;
                context.ExceptionHandled = true;
            }
        }
    }
}
