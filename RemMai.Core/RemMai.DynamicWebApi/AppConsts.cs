using RemMai.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RemMai.DynamicWebApi;
internal static class AppConsts
{
    /// <summary>
    /// 默认请求方式
    /// </summary>
    public static string DefaultHttpVerb { get; set; }
    /// <summary>
    /// 默认区域名称
    /// </summary>
    public static string DefaultAreaName { get; set; }

    /// <summary>
    /// 默认Api路由前缀
    /// </summary>
    public static string DefaultApiPreFix { get; set; }

    /// <summary>
    /// 控制器后缀
    /// </summary>
    public static List<string> ControllerPostfixes { get; set; }

    /// <summary>
    /// 方法名后缀
    /// </summary>
    public static List<string> ActionPostfixes { get; set; }

    /// <summary>
    /// 请求体忽略类型
    /// </summary>
    public static List<Type> FormBodyBindingIgnoredTypes { get; set; }

    /// <summary>
    /// Http请求方式
    /// </summary>
    public static Dictionary<string, string> HttpVerbs { get; set; }


    /// <summary>
    /// 通过Action的名字来判断RestFul的请求方式
    /// <para></para>
    /// 以请求方式属性为主
    /// </summary>
    public static Func<string, string> GetRestFulActionName { get; set; }
    static AppConsts()
    {
        HttpVerbs = new Dictionary<string, string>()
        {
            ["add"] = "POST",
            ["create"] = "POST",
            ["post"] = "POST",

            ["select"] = "GET",
            ["list"] = "GET",
            ["get"] = "GET",
            ["find"] = "GET",
            ["fetch"] = "GET",
            ["query"] = "GET",

            ["update"] = "PUT",
            ["put"] = "PUT",

            ["delete"] = "DELETE",
            ["remove"] = "DELETE",
        };
    }
}
