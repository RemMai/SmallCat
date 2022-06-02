using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SmartCat;

/// <summary>
/// Props
/// </summary>
public static class Cat
{
    public static IWebHostEnvironment? Environment { get; internal set; }
    public static ConfigurationManager? ConfigurationManager { get; internal set; }
    public static IConfiguration? Configuration { get => ConfigurationManager; }
    public static string ExecuteDirectory { get; } = AppContext.BaseDirectory;
    public static List<Assembly> ProjectAssemblies { get { if (Init._assemblies == null) { Init._assemblies = Init.GetProjectAssemblies(); } return Init._assemblies; } }
    public static IServiceCollection? Services { get; internal set; }
    public static HttpContext? HttpContext => GetService<IHttpContextAccessor>()!.HttpContext;
    public static ServiceProvider? ServiceProvider { get; internal set; }
    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T GetService<T>() => ServiceProvider!.GetService<T>() ?? throw new Exception($"Not Find {typeof(T).Name} Service!");
}