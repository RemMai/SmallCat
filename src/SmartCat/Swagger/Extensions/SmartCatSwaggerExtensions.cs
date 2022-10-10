using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

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
    public static IServiceCollection AddSmartCatSwagger(this IServiceCollection services, Action<SwaggerGenOptions>? swaggerGenOptions = null)
    {
        swaggerGenOptions ??= options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Dynamic OpenApi", Version = "v1" });

            // TODO：一定要返回true！
            options.DocInclusionPredicate((_, _) => true);

            // TODO：不可省略，否则Swagger中文注释不显示。
            DirectoryInfo directoryInfo = new(AppContext.BaseDirectory);

            directoryInfo.GetFiles("*.xml").Select(e => e.FullName).ToList().ForEach(xmlFullName =>
            {
                // 添加控制器层注释，true表示显示控制器注释
                options.IncludeXmlComments(xmlFullName, true);
            });
            options.OperationFilter<AddResponseHeadersFilter>();
            options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>(true, JwtBearerDefaults.AuthenticationScheme);

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Type = SecuritySchemeType.ApiKey
            });
        };

        // 注册Swagger
        services.AddSwaggerGen(swaggerGenOptions);

        return services;
    }


    public static IApplicationBuilder UseSmartCatSwaggerUi(this IApplicationBuilder app, Action<SwaggerUIOptions>? swaggerUiAction = null, Action<SwaggerOptions>? swaggerAction = null)
    {
        if (swaggerUiAction != null)
        {
            app.UseSwaggerUI(swaggerUiAction);
        }
        else
        {
            app.UseSwaggerUI(options =>
            {
                options.IndexStream = () => Assembly.GetExecutingAssembly().GetManifestResourceStream("SmartCat.Swagger.UI.index.html");
                options.RoutePrefix = "swagger";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            });
        }

        app.UseSwagger(swaggerAction);
        return app;
    }
}