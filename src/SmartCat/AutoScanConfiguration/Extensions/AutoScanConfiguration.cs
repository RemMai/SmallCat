using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.Extensions.AutoScanConfiguration
{
    public static class AutoScanConfiguration
    {

        /// <summary>
        /// 扫描Json配置文件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AutoScanConfigurationFile(this WebApplicationBuilder builder, List<string>? filterJosnFileList = null)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            // 过滤的Json文件
            List<string> filterJsonName = filterJosnFileList ?? new List<string>()
                    {
                        "appsettings.Development.json",
                        "appsettings.json",
                        ".deps.json",
                        ".runtimeconfig.json",
                    };

            List<string> jsonfiles = Directory.GetFiles(path, "*.json").OrderBy(e => e == "SmartCatSetting.Json" ? 1 : 2).Where(fileName =>
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

            foreach (string jsonfile in jsonfiles)
            {
                Cat.ConfigurationManager.AddJsonFile(jsonfile, optional: true, reloadOnChange: true);
            }

            return builder;
        }

    }
}
