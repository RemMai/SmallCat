using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Helpers;
using SmartCat.Model;
using SmartCat.UnifiedResponse.Attribute;

namespace SmartCat.UnifiedResponse;

internal static class UnifiedResponseContextHelper
{
    /// <summary>
    /// 参数检查并且序列化参数的错误信息。
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static ActionExecutingContext DataValidation(this ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return context;
        var errors = new List<string>();

        foreach (var item in context.ModelState.Values)
        {
            errors.AddRange(item.Errors.Select(error => error.ErrorMessage));
        }

        var result = new RestFulResult<object>()
        {
            Success = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Message = "请求参数错误，详细信息请查看Error",
            Error = errors
        };
        context.Result = new JsonResult(result);

        return context;
    }

    /// <summary>
    /// 判断跳过RestFul序列化响应
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static bool SkipUnifiedResponse(this ActionExecutedContext context)
    {
        var controller = (ControllerActionDescriptor)context.ActionDescriptor;
        var method = controller.MethodInfo;
        var type = controller.ControllerTypeInfo;
        return SkipUnifiedResponse(method.SkipUnifiedResponseByMethodInfo(), type.SkipUnifiedResponseByTypeInfo());
    }

    /// <summary>
    /// 跳过统一响应
    /// </summary>
    /// <param name="methodEnable"></param>
    /// <param name="typeEnable"></param>
    /// <returns></returns>
    public static bool SkipUnifiedResponse(bool? methodEnable, bool? typeEnable)
    {
        if (methodEnable.HasValue)
        {
            return methodEnable.Value;
        }

        return typeEnable.HasValue && typeEnable.Value;
    }


    /// <summary>
    /// 异常处理
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static ExceptionContext UnifiedException(this ExceptionContext context)
    {
        if (context.Exception != null)
        {
            var result = new RestFulResult<object>()
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
            context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            context.ExceptionHandled = true;
        }

        return context;
    }


    public static bool? SkipUnifiedResponseByMethodInfo(this MethodInfo methodInfo)
    {
        var attr = methodInfo.GetSingleAttributeOrNull<NonRestFulAttribute>();
        return attr.Enable;
    }

    public static bool? SkipUnifiedResponseByTypeInfo(this TypeInfo type)
    {
        var attr = type.GetSingleAttributeOrNull<NonRestFulAttribute>();
        return attr.Enable;
    }
}