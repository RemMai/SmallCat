using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SmartCat.Model;

namespace SmartCat.JwtAuthorization;

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
                401 => "请登录",
                404 => "未找到服务",
                500 => "服务内部错误",
                502 => "请求错误",
                _ => "其他错误"
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
        if (context.Response.HasStarted) return Task.CompletedTask;
        context.Response.StatusCode = 200;
        context.Response.WriteAsJsonAsync(new RestFulResult<string>() { StatusCode = statusCode, Error = msg });
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