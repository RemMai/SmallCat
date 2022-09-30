using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SmartCat.AutoDi.Extensions;
using SmartCat.AutoScanConfiguration.Extensions;
using SmartCat.DynamicWebApi.Extensions;
using SmartCat.Extensions.SmartCatMiniProfiler;
using SmartCat.Extensions.Swagger;
using SmartCat.JwtAuthorization;
using SmartCat.RestFul;

namespace SmartCat;

public static class GlobalObjectInjectExtensions
{
    /// <summary>
    /// 注入SmartCat Cat 全局静态变量
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="smartCatOption"></param>
    /// <returns></returns>
    public static WebApplicationBuilder InitSmartCat(this WebApplicationBuilder appBuilder, Action<SmartCatOptions>? smartCatOption = null)
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

        appBuilder.Services.AddAutoDi();

        return appBuilder;
    }

    public static IServiceCollection InjectSmartCat(this IMvcBuilder mvcBuilder)
    {
        // 全局服务注入 
        mvcBuilder.Services.AddDynamicWebApi();
        mvcBuilder.Services.AddSmartCatSwagger();
        mvcBuilder.Services.AddSmartCatMiniProfiler();
        mvcBuilder.Services.AddEndpointsApiExplorer();

        mvcBuilder.Services.Configure<MvcOptions>(option =>
        {
            option.Filters.Add<SimpleActionFilter>(1);
            option.Filters.Add<SimpleAsyncActionFilter>(2);
            option.Filters.Add<SimpleExceptionFilter>(3);
        });

        mvcBuilder.AddJsonOptions(option =>
        {
            option.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

        return mvcBuilder.Services;
    }


    public static IApplicationBuilder UseSmartCat(this IApplicationBuilder app)
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
