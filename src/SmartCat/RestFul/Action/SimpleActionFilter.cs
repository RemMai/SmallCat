using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Model;

namespace SmartCat.RestFul;

public class SimpleActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.DataValidation();
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Canceled) return;

        var data = context.Result switch
        {
            ContentResult contentResult => new KeyValuePair<bool, object>(true, contentResult.Content),
            JsonResult jsonResult => new KeyValuePair<bool, object>(true, jsonResult.Value),
            ObjectResult _object => new KeyValuePair<bool, object>(true, _object.Value),
            _ => new KeyValuePair<bool, object>(false, null),
        };
        if (data.Key)
        {
            if (!context.SkipRestFul())
            {
                context.Result = new JsonResult(new RestFulResult<object>() { StatusCode = StatusCodes.Status200OK, Message = null, Success = true, Data = data.Value });
            }
        }

        context.Canceled = true;
    }
}