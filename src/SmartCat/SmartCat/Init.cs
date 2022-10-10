using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace SmartCat;

internal static class Init
{

    /// <summary>
    /// 获取当前项目所有自己创建的程序集
    /// </summary>
    /// <returns></returns>
    internal static List<Assembly> GetProjectAssemblies()
    {
        // 发布环境使用。
        var context = DependencyContext.Default.RuntimeLibraries;
        
        var projectAssemblyNames = context.Where(e => e.Type == "project").Select(e => e.Name);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(e => projectAssemblyNames.Contains(e.GetName().Name ?? "Unknow")).ToList();

        return assemblies;

    }
}
