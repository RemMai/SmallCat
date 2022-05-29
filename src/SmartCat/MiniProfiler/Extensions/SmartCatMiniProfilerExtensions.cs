using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace SmartCat.Extensions.MiniProfiler;
/// <summary>
/// 注册动态Api
/// <para></para>
/// PandaDynamicWebApi
/// <para></para>
/// Swagger
/// </summary>
public static class SmartCatMiniProfilerExtensions
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