using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmallCat.Model;
using SmallCat;

namespace SmallCat;

/// <summary>
/// Props
/// </summary>
public static class Cat
{
    static Cat()
    {
        ProjectAssemblies = Init.GetProjectAssemblies();
        Db = new IdleBus<IFreeSql>(TimeSpan.FromMinutes(5));
    }

    public static IWebHostEnvironment? Environment { get; internal set; }
    public static ConfigurationManager? ConfigurationManager { get; internal set; }
    public static IConfiguration? Configuration => ConfigurationManager;
    public static string ExecuteDirectory { get; } = AppContext.BaseDirectory;
    public static List<Assembly> ProjectAssemblies { get; }
    public static IServiceCollection? Services { get; internal set; }
    public static HttpContext? HttpContext => GetService<IHttpContextAccessor>().HttpContext;
    public static ServiceProvider? ServiceProvider { get; internal set; }
    public static IdleBus<IFreeSql> Db { get; }
    public static JwtSetting JwtSetting { get; internal set; }

    /// <summary>
    /// 获取服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T GetService<T>() => ServiceProvider!.GetService<T>() ?? throw new Exception($"Not Find \"{typeof(T).Name}\" Service!");


    /// <summary>
    /// FreeSqlDataBase
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static IFreeSql GetDb(string name = "IDbLocker")
    {
        var db = Db.Get(name);

        return db ?? throw SmallCatMiao.Gugu($"Not found Db '{name}'!", 500);
    }
}