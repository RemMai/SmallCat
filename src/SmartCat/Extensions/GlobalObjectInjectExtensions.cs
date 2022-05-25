using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCat.Extensions;
using SmartCat.Extensions.Swagger;
using SmartCat.Extensions.DynamicWebApi;
using SmartCat.Extensions.Authorization;
using SmartCat.Extensions.IocAutoInject;
using SmartCat.Extensions.MiniProfiler;

namespace SmartCat;

public static class GlobalObjectInjectExtensions
{
    public static WebApplicationBuilder InitSmartCat(this WebApplicationBuilder appBuilder, Action<SmartCatOptions>? smartCatOption = null)
    {
        // 全局服务注入 
        appBuilder.Services.AutoInject();

        // 注册Configuration
        Cat.ConfigurationManager = appBuilder.Configuration;

        // 注册Environment
        Cat.Environment = appBuilder.Environment;

        // 注册全局请求上下文
        appBuilder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // 自动扫描Json配置文件
        appBuilder.AutoScanConfigurationFile();

        var options = new SmartCatOptions();
        smartCatOption?.Invoke(options);
        // 添加路由，且添加Swagger;
        appBuilder.Services.AddControllers();
        appBuilder.Services.AddDynamicWebApi(options.DynamicWebApiConfiguration);
        appBuilder.Services.AddEndpointsApiExplorer();
        appBuilder.Services.AddSmartCatMiniProfiler();
        appBuilder.Services.AddSmartCatSwagger(options.SwaggerGenOptions);

        // 注册服务集合
        Cat.Services = appBuilder.Services;

        // 注册服务提供器
        Cat.ServiceProvider = appBuilder.Services.BuildServiceProvider(false);

        return appBuilder;
    }
    public static WebApplicationBuilder InitSmartCat<THandler>(this WebApplicationBuilder appBuilder, Action<SmartCatOptions>? smartCatOption = null) where THandler : class, IAuthorizationHandler
    {
        appBuilder.InitSmartCat(smartCatOption);
        appBuilder.Services.AddJwtAuthorization<THandler>();
        return appBuilder;
    }


    public static IApplicationBuilder UseSmartCat(this IApplicationBuilder app)
    {
        app.UseSmartCatMiniProfiler();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapSwagger();
        });
        app.UseSmartCatSwaggerUI();
        return app;
    }
}
