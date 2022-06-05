﻿/**
    这个地方有更好的实现方式，待开发。。。现在玩的比较简单.
 */


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Model;

namespace SmartCat.RestFul;

public abstract class SimpleAsyncAuthorizationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!CheckAuthorization(context))
        {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new JsonResult(NoAuthorizationResult());
            await Task.Run(() => { });
        }
    }
    public virtual bool CheckAuthorization(AuthorizationFilterContext context) => true;
    public virtual object NoAuthorizationResult() => new RestFulResult<object>() { Message = "The Request NoAuthorization!", StatusCode = 401, Success = false };
}
