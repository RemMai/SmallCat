using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemMai;

public static class RemMaiApp
{
    public static ConfigurationManager ConfigurationManager { get; set; }
    public static IConfiguration Configuration
    {
        get { return ConfigurationManager; }
    }


    private static string _executeDirectory { get; set; }
    public static string ExecuteDirectory
    {
        get
        {
            if (string.IsNullOrEmpty(_executeDirectory))
            {
                _executeDirectory = AppContext.BaseDirectory;
            }
            return _executeDirectory;
        }
    }

    public static bool ReLoadConfiguration()
    {
        return true;
    }
}
