using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace SmartCat;

internal static class Init
{
    /// <summary>
    /// 获取当前项目所有自己创建的程序集
    /// </summary>
    /// <returns></returns>
    public static List<Assembly> GetProjectAssemblies()
    {
        var deps = DependencyContext.Default;

        var projectAssemblies = deps.CompileLibraries.Where(e => e.Type == "project").Select(e => e.Name).ToList();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(e => projectAssemblies.Contains(e.GetName().Name ?? "Unknow")).ToList();

        return assemblies;
    }
}
