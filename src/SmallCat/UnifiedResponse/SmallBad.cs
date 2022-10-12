using Microsoft.AspNetCore.Http;

namespace SmallCat;

/// <summary>
/// Miao （喵）
/// </summary>
public static class SmallCatMiao
{
    /// <summary>
    /// 全局异常处理（Gugu，肚子饿了，能不出问题?）
    /// </summary>
    /// <returns></returns>
    public static SmallCatException Gugu(string message, int? statusCode = 500) => statusCode.HasValue ? new SmallCatException(message, statusCode.Value) : new SmallCatException(message);
}

public class SmallCatException : Exception
{
    /// <summary>
    /// 全局异常模型
    /// </summary>
    /// <param name="message">报错信息</param>
    public SmallCatException(string message) : base(message)
    {
        Message = message;
    }
    public SmallCatException(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }

    public int StatusCode { get; } = StatusCodes.Status500InternalServerError;
    public override string Message { get; }
}