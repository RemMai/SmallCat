using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Filter.Authorize;
using SmartCat.Filter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.Filter.Action;

public class SimpleActionFilter : IActionFilter //, IAsyncActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.DataValidation();
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (!context.Canceled)
        {
            var data = context.Result switch
            {
                ContentResult contentResult => new KeyValuePair<bool, object>(true, contentResult.Content),
                JsonResult jsonResult => new KeyValuePair<bool, object>(true, jsonResult.Value),
                ObjectResult _object => new KeyValuePair<bool, object>(true, _object.Value),
                _ => new KeyValuePair<bool, object>(false, null),
            };
            if (data.Key)
            {
                context.Result = new JsonResult(new DefaultJsonResult() { StatusCode = StatusCodes.Status200OK, Message = null, Success = true, Data = data.Value });
            }

            context.Canceled = true;
        }
    }


}