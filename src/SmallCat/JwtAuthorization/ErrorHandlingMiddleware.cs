using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SmallCat.Model;

namespace SmallCat.JwtAuthorization;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
            var statusCode = context.Response.StatusCode;

            var msg = statusCode switch
            {
                200 => string.Empty,
                302 => "页面跳转中...",
                401 => "未授权访问",
                404 => "未找到该服务",
                500 => "程序内部错误",
                502 => "请求方式错误",
                _ => "其他未知错误，请联系管理员",
            };
            if (!string.IsNullOrWhiteSpace(msg))
            {
                await HandleExceptionAsync(context, statusCode, msg);
            }
        }
        catch (Exception ex)
        {
            var statusCode = context.Response.StatusCode;
            await HandleExceptionAsync(context, statusCode, ex.Message);
        }
    }

    //异常错误信息捕获，将错误信息用Json方式返回
    private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = statusCode;
            context.Response.WriteAsJsonAsync(new UnifiedResult<string> { StatusCode = statusCode, Error = msg });
        }
        return Task.CompletedTask;
    }
}

//扩展方法
public static class ErrorHandlingExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}