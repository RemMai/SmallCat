﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace SmallCat.UnifiedResponse.Filter;

public class SmartCatAsyncActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next();
    }
}
