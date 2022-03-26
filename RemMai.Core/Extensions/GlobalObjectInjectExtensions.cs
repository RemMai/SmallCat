using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemMai.Extensions;

public static class GlobalObjectInjectExtensions
{
    public static WebApplicationBuilder GlobalObjectInject(this WebApplicationBuilder builder, bool autoScan = true)
    {
        if (RemMaiApp.ConfigurationManager == null)
        {
            RemMaiApp.ConfigurationManager = builder.Configuration;
        }
        builder.AutoScanConfigurationFile();
        return builder;
    }

    private static WebApplicationBuilder AutoScanConfigurationFile(this WebApplicationBuilder builder)
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;

        // 过滤的Json文件
        List<string> filterJsonName = new List<string>()
        {
            "appsettings.Development.json",
            "appsettings.json",
            ".deps.json",
            ".runtimeconfig.json",
        };
        List<string> jsonfiles = Directory.GetFiles(path, "*.json").ToList().Where(fileName =>
        {
            bool result = false;

            foreach (string filterName in filterJsonName)
            {
                result = Path.GetFileName(fileName).ToLower().Contains(filterName.ToLower());
                if (result)
                {
                    break;
                }
            }
            return !result;
        }).ToList();

        string environmentName = builder.Environment.EnvironmentName;

        foreach (string jsonfile in jsonfiles)
        {
            RemMaiApp.ConfigurationManager.AddJsonFile(jsonfile, optional: true, reloadOnChange: true);
        }

        return builder;
    }
}
