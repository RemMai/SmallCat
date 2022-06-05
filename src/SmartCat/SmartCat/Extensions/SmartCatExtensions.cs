using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCat.Extensions.AutoScanConfiguration;
using SmartCat.Extensions.DynamicWebApi;
using SmartCat.Extensions.IocAutoInject;
using SmartCat.Extensions.MiniProfiler;
using SmartCat.Extensions.Swagger;
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

        // 注册服务集合
        Cat.Services = appBuilder.Services;

        // 注册服务提供器
        Cat.ServiceProvider = appBuilder.Services.BuildServiceProvider(false);


        appBuilder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Cat.ConfigurationManager.Bind("JwtSettings", options));

        appBuilder.Services.AddAuthorization();

        return appBuilder;
    }

    public static IServiceCollection InjectSmartCat(this IMvcBuilder mvcBuilder, Action<SmartCatOptions>? smartCatOption = null, Action<IServiceCollection>? serviceCollection = null)
    {
        var options = new SmartCatOptions();
        smartCatOption?.Invoke(options);
        mvcBuilder.InjectSmartCat(options);
        // 注册其他服务
        serviceCollection?.Invoke(mvcBuilder.Services);
        return mvcBuilder.Services;
    }
    private static IServiceCollection InjectSmartCat(this IMvcBuilder mvcBuilder, SmartCatOptions? options = null)
    {
        mvcBuilder.Services.AddDynamicWebApi(options.DynamicWebApiConfiguration);
        mvcBuilder.Services.AddSmartCatSwagger(options.SwaggerGenOptions);
        mvcBuilder.Services.AddSmartCatMiniProfiler();
        mvcBuilder.Services.AddEndpointsApiExplorer();

        mvcBuilder.Services.Configure<MvcOptions>(option =>
        {
            option.Filters.Add<SimpleActionFilter>();
            option.Filters.Add<SimpleAsyncActionFilter>();
            option.Filters.Add<SimpleExceptionFilter>();
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
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSmartCatSwaggerUI();
        app.UseSmartCatMiniProfiler();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapSwagger();
        });
        return app;
    }
}
