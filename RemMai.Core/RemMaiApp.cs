using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RemMai;

public static class RemMaiApp
{
    public static ConfigurationManager ConfigurationManager { get; set; }
    public static IConfiguration Configuration { get => ConfigurationManager; }
    public static string ExecuteDirectory { get; } = AppContext.BaseDirectory;
    public static List<Assembly> ProjectAssemblies { get; } = Init.GetProjectAssemblies();

    public static bool ReLoadConfiguration()
    {
        return true;
    }
}
