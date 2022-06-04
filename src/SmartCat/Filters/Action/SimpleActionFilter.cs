using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Filter.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.Filter.DataValidation;

public class SimpleActionFilter : IActionFilter, IAsyncActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("Execute End!");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("Execute Start!");
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next();
    }
}
