using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SmartCat.Filter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.Filter.Result
{
    public class SimpleResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.Cancel)
            {

                var data = context.Result switch
                {
                    ContentResult contentResult => contentResult.Content,
                    JsonResult jsonResult => jsonResult.Value,
                    ObjectResult _object => _object.Value,
                    _ => null
                };
                if (data != null)
                {
                    context.Result = new JsonResult(new DefaultJsonResult() { StatusCode = StatusCodes.Status200OK, Message = null, Success = true, Data = data });
                }
            }

        }
    }
}
