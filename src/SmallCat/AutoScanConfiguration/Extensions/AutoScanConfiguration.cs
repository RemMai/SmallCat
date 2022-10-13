using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace SmallCat.AutoScanConfiguration.Extensions
{
    public static class AutoScanConfiguration
    {
        /// <summary>
        /// 扫描Json配置文件
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="filterJsonFileList"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AutoScanConfigurationFile(this WebApplicationBuilder builder, List<string>? filterJsonFileList = null)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            var smallCatJsonConfiguration = Helpers.ResourceHelper.GetJsonResources();
            smallCatJsonConfiguration.ForEach(e =>
            {
                Cat.ConfigurationManager.AddJsonStream(e);
                Cat.ConfigurationManager.GetSection("JwtSettings").Get<Model.JwtSetting>();
            });

            // 自定义 Configuration Files
            // 需要过滤的Json文件
            var filterJsonName = filterJsonFileList ?? new List<string>()
            {
                "appsettings.Development.json",
                ".runtimeconfig.json",
            };

            Directory.GetFiles(path, "*.json")
                .Where(fileName => filterJsonName.Any(fileName.EndsWith))
                .ToList()
                .ForEach(e => Cat.ConfigurationManager.AddJsonFile(e, optional: true, reloadOnChange: true));

            return builder;
        }
    }
}