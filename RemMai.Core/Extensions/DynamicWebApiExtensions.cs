using Panda.DynamicWebApi;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore;
using Microsoft.OpenApi.Models;

namespace RemMai.Extensions;
/// <summary>
/// 注册动态Api
/// <para></para>
/// PandaDynamicWebApi
/// <para></para>
/// Swagger
/// </summary>
public static class DynamicWebApiExtensions
{
    /// <summary>
    /// 注册动态Api
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDynamicWebApi(this IServiceCollection services)
    {
        // 注册Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Panda OpenApi", Version = "v1" });

            // TODO：一定要返回true！
            options.DocInclusionPredicate((docName, description) => true);

            // TODO：不可省略，否则Swagger中文注释不显示。
            DirectoryInfo directoryInfo = new(AppContext.BaseDirectory);
            directoryInfo.GetFiles("*.xml").Select(e => e.FullName).ToList().ForEach(xmlFullName =>
            {
                options.IncludeXmlComments(xmlFullName, true);
            });
        });

        // 注册PandaDynamicWebApi
        services.AddDynamicWebApi(options =>
        {
            RemMaiApp.ProjectAssemblies.ForEach(assembly =>
            {
                options.AssemblyDynamicWebApiOptions.Add(assembly, new AssemblyDynamicWebApiOptions());
            });
        });
        return services;
    }
}

