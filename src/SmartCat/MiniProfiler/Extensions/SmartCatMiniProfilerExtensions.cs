using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;

namespace SmartCat.Extensions.Swagger;
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
        services.AddMiniProfiler(options => { options.RouteBasePath = "/profiler";  });
        return services;
    }

    public static IApplicationBuilder UseSmartCatMiniProfiler(this IApplicationBuilder app)
    {
        app.UseMiniProfiler();
        return app;
    }
}

