using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat;

/// <summary>
/// Props
/// </summary>
public static partial class Cat
{
    public static IWebHostEnvironment? Environment { get; internal set; }
    public static ConfigurationManager? ConfigurationManager { get; internal set; }
    public static IConfiguration? Configuration { get => ConfigurationManager; }
    public static string ExecuteDirectory { get; } = AppContext.BaseDirectory;
    public static List<Assembly> ProjectAssemblies { get; } = Init.GetProjectAssemblies();
    public static IServiceCollection? Services { get; internal set; }
    public static HttpContext? HttpContext => GetService<IHttpContextAccessor>()!.HttpContext;
    public static ServiceProvider? ServiceProvider { get; internal set; }
    internal static bool GlobaAuthorization { get; set; } = true;
}

/// <summary>
/// Acitons
/// </summary>
public static partial class Cat
{
    public static T GetService<T>() => ServiceProvider!.GetService<T>() ?? throw new Exception($"Not Find {typeof(T).Name} Service!");
}



