using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RemMai;

internal class Init
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
