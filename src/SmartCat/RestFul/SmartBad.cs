using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat;

/// <summary>
/// Miao （喵）
/// </summary>
public static class SmartCatMiao
{
    /// <summary>
    /// 全局异常处理（Gugu，肚子饿了，能不出问题?）
    /// </summary>
    /// <returns></returns>
    public static SmartCatException Gugu(string message, int? statusCode) => statusCode.HasValue ? new SmartCatException(message, statusCode.Value) : new SmartCatException(message);
}

public class SmartCatException : Exception
{
    public SmartCatException() : base() { }

    /// <summary>
    /// 全局异常模型
    /// </summary>
    /// <param name="message">报错信息</param>
    public SmartCatException(string message) : base(message)
    {
        Message = message;
    }
    public SmartCatException(string message, int statusCode) : base()
    {
        Message = message;
        StatusCode = statusCode;
    }

    public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;
    public override string Message { get; }
}