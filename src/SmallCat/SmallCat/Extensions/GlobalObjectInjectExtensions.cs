using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SmallCat.Extensions.SmallCatMiniProfiler;
using SmallCat.AutoScanConfiguration.Extensions;
using SmallCat.DynamicWebApi.Extensions;
using SmallCat.Extensions.Swagger;
using SmallCat.JwtAuthorization;
using SmallCat.ServiceAutoDiscover.Extensions;
using SmallCat.UnifiedResponse.Exception;
using SmallCat.UnifiedResponse.Filter;

namespace SmallCat;

public static class GlobalObjectInjectExtensions
{
    /// <summary>
    /// 注入 Small Cat 全局静态变量
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="smartCatOption"></param>
    /// <returns></returns>
    public static WebApplicationBuilder InitSmallCat(this WebApplicationBuilder appBuilder, Action<SmallCatOptions>? smartCatOption = null)
    {
        // 注册全局请求上下文
        appBuilder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // 注册Configuration
        Cat.ConfigurationManager = appBuilder.Configuration;

        // 注册Environment
        Cat.Environment = appBuilder.Environment;

        // 注册服务集合
        Cat.Services = appBuilder.Services;

        // 注册服务提供器
        Cat.ServiceProvider = appBuilder.Services.BuildServiceProvider(false);

        // 自动扫描Json配置文件
        appBuilder.AutoScanConfigurationFile();

        appBuilder.Services.AddServiceAutoDiscover();

        return appBuilder;
    }

    public static IServiceCollection InjectSmallCat(this IMvcBuilder mvcBuilder)
    {
        // 全局服务注入 
        mvcBuilder.Services.AddDynamicWebApi();
        mvcBuilder.Services.AddSmartCatSwagger();
        mvcBuilder.Services.AddSmartCatMiniProfiler();
        mvcBuilder.Services.AddEndpointsApiExplorer();

        mvcBuilder.Services.Configure<MvcOptions>(option =>
        {
            option.Filters.Add<SmartCatActionFilter>(1);
            option.Filters.Add<SmartCatAsyncActionFilter>(2);
            option.Filters.Add<SmartCatExceptionFilter>(3);
        });

        mvcBuilder.AddJsonOptions(option => { option.JsonSerializerOptions.PropertyNamingPolicy = null; });

        return mvcBuilder.Services;
    }


    public static IApplicationBuilder UseSmallCat(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseErrorHandling();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSmartCatSwaggerUi();
        app.UseSmartCatMiniProfiler();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapSwagger();
        });
        return app;
    }
}