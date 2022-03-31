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

namespace RemMai;

/// <summary>
/// Props
/// </summary>
public static partial class RemMaiApp
{
    public static IWebHostEnvironment Environment { get; internal set; }
    public static ConfigurationManager ConfigurationManager { get; internal set; }
    public static IConfiguration Configuration { get => ConfigurationManager; }
    public static string ExecuteDirectory { get; } = AppContext.BaseDirectory;
    public static List<Assembly> ProjectAssemblies { get; } = Init.GetProjectAssemblies();
    public static IServiceCollection Services { get; internal set; }

    public static HttpContext HttpContext { get; internal set; }
    public static ServiceProvider ServiceProvider { get; internal set; }
}

/// <summary>
/// Acitons
/// </summary>
public static partial class RemMaiApp
{
    public static T GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }
}



