using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RemMai.Helpers;

namespace RemMai.DynamicWebApi;
/// <summary>
/// Add Dynamic WebApi
/// </summary>
public static class DynamicWebApiServiceExtensions
{
    /// <summary>
    /// Add Dynamic WebApi to Container
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options">configuration</param>
    /// <returns></returns>
    private static IServiceCollection AddDynamicWebApi(this IServiceCollection services, DynamicWebApiOptions options)
    {
        if (options == null)
        {
            throw new ArgumentException(nameof(options));
        }

        options.Valid();

        AppConsts.DefaultAreaName = options.DefaultAreaName;
        AppConsts.DefaultHttpVerb = options.DefaultHttpVerb;
        AppConsts.DefaultApiPreFix = options.DefaultApiPrefix;
        AppConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
        AppConsts.ActionPostfixes = options.RemoveActionPostfixes;
        AppConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
        AppConsts.GetRestFulActionName = options.GetRestFulActionName;

        var partManager = services.GetSingletonInstanceOrNull<ApplicationPartManager>();
        // Add a custom controller checker

        partManager.FeatureProviders.Add(new DynamicWebApiControllerFeatureProvider(options.DynamicController));

        services.Configure<MvcOptions>(o =>
        {
            // Register Controller Routing Information Converter
            o.Conventions.Add(new DynamicWebApiConvention(options.DynamicController, options.ActionRouteFactory));
        });

        return services;
    }

    public static IServiceCollection AddDynamicWebApi(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder.Services.AddDynamicWebApi(new DynamicWebApiOptions());
    }

    public static IServiceCollection AddDynamicWebApi(this IMvcBuilder mvcBuilder, Action<DynamicWebApiOptions> optionsAction)
    {
        var dynamicWebApiOptions = new DynamicWebApiOptions();

        optionsAction?.Invoke(dynamicWebApiOptions);

        return mvcBuilder.Services.AddDynamicWebApi(dynamicWebApiOptions);
    }

}
