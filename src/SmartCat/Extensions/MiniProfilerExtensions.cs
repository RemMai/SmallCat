using Microsoft.Extensions.DependencyInjection;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.Extensions.MiniProfiler;

public static class MiniProfilerExtensions
{
    public static IServiceCollection AddSmartCatMiniProfiler(this IServiceCollection services, Action<MiniProfilerOptions>? configureOptions = null)
    {

        services.AddMiniProfiler(options =>
        {
            options.RouteBasePath = "/";
            options.EnableMvcFilterProfiling = false;
            options.EnableMvcViewProfiling = false;
        });

        configureOptions?.Invoke(new MiniProfilerOptions());

        return services;
    }
}
