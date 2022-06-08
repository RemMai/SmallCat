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

            var smartCatJsonConfiguration = Helpers.ResourceHelper.GetJsonResources();
            smartCatJsonConfiguration.ForEach(e =>
            {
                Cat.ConfigurationManager.AddJsonStream(e);
                var data = Cat.ConfigurationManager.GetSection("JwtSettings").Get<Model.JwtSetting>();
            });

            // 自定义 Configuration Files //
            // 需要过滤的Json文件
            List<string> filterJsonName = filterJosnFileList ?? new List<string>()
            {
                "appsettings.Development.json",
                ".deps.json",
                ".runtimeconfig.json",
            };

            var data = Directory.GetFiles(path, "*.json");

            data.ToList().ForEach(e => Console.WriteLine(e));

            Directory.GetFiles(path, "*.json")
                .Where(fileName => filterJsonName.Any(t => fileName.EndsWith(t)))
                .ToList()
                .ForEach(e=> Cat.ConfigurationManager.AddJsonFile(e, optional: true, reloadOnChange: true));

            return builder;
        }

    }
}
