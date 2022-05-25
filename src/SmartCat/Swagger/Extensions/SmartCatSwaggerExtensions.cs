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
public static class SmartCatSwaggerExtensions
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSmartCatSwagger(this IMvcBuilder mvcBuilder, Action<SwaggerGenOptions>? swaggerGenOptions = null)
    {
        // 注册Swagger
        mvcBuilder.Services.AddSmartCatSwagger(swaggerGenOptions);

        return mvcBuilder.Services;
    }

    public static IServiceCollection AddSmartCatSwagger(this IServiceCollection services, Action<SwaggerGenOptions>? swaggerGenOptions = null)
    {
        if (swaggerGenOptions == null)
        {
            swaggerGenOptions = options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Dynamic OpenApi", Version = "v1" });

                // TODO：一定要返回true！
                options.DocInclusionPredicate((docName, description) => true);

                // TODO：不可省略，否则Swagger中文注释不显示。
                DirectoryInfo directoryInfo = new(AppContext.BaseDirectory);
                directoryInfo.GetFiles("*.xml").Select(e => e.FullName).ToList().ForEach(xmlFullName =>
                {
                    // 添加控制器层注释，true表示显示控制器注释
                    options.IncludeXmlComments(xmlFullName, true);
                });

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new List<string>()
                    }
                });
            };
        }
        // 注册Swagger
        services.AddSwaggerGen(swaggerGenOptions);

        return services;
    }


    public static IApplicationBuilder UseSmartCatSwaggerUI(this IApplicationBuilder app)
    {
        Console.WriteLine(typeof(SmartCatOptions).Assembly.FullName);
        app.UseSwaggerUI(options =>
        {
            var steam = typeof(SmartCatOptions).Assembly.GetManifestResourceStream("SmartCat.Swagger.UI.index.html");
            options.IndexStream = () => typeof(SmartCatOptions).Assembly.GetManifestResourceStream("SmartCat.Swagger.UI.index.html");
            options.RoutePrefix = "swagger";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
        });

        return app;
    }
}

