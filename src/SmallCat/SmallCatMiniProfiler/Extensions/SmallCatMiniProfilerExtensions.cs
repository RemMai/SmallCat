using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmallCat;

namespace SmallCat.Extensions.SmallCatMiniProfiler;

/// <summary>
/// 注册动态Api
/// <para></para>
/// PandaDynamicWebApi
/// <para></para>
/// Swagger
/// </summary>
public static class SmallCatMiniProfilerExtensions
{
    public static IServiceCollection AddSmartCatMiniProfiler(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider(false).GetService<IConfiguration>();

        var isEnable = configuration.GetSection("MiniProfiler").Value;

        if (isEnable == null || configuration.GetValue<bool>("MiniProfiler"))
        {
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
                options.EnableMvcFilterProfiling = false;
                options.EnableMvcViewProfiling = false;
                options.EnableDebugMode = Cat.Environment.IsDevelopment();
            });
        }

        return services;
    }

    public static IApplicationBuilder UseSmartCatMiniProfiler(this IApplicationBuilder app)
    {
        var configuration = app.ApplicationServices.GetService<IConfiguration>();

        var isEnable = configuration.GetSection("MiniProfiler").Value;

        if (isEnable == null || configuration.GetValue<bool>("MiniProfiler")) app.UseMiniProfiler();

        return app;
    }
}