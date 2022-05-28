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
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SmartCat.Model;
using Microsoft.IdentityModel.Tokens;

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

        return appBuilder;
    }

    public static IServiceCollection InjectSmartCat<THandler>(this IMvcBuilder mvcBuilder, Action<SmartCatOptions>? smartCatOption = null) where THandler : class, IAuthorizationHandler
    {
        var options = new SmartCatOptions();
        smartCatOption?.Invoke(options);
        mvcBuilder.InjectSmartCat<THandler>(options);
        return mvcBuilder.Services;
    }
    public static IServiceCollection InjectSmartCat(this IMvcBuilder mvcBuilder, Action<SmartCatOptions>? smartCatOption = null)
    {
        var options = new SmartCatOptions();
        smartCatOption?.Invoke(options);
        mvcBuilder.InjectSmartCat(options);
        return mvcBuilder.Services;
    }
    private static IServiceCollection InjectSmartCat<THandler>(this IMvcBuilder mvcBuilder, SmartCatOptions? options = null) where THandler : class, IAuthorizationHandler
    {
        if (options.GlobaAuthorization)
        {
            mvcBuilder.Services.AddJwtAuthorization<THandler>(options.AuthenticationConfigure, options.JwtBearerConfigure);

            mvcBuilder.Services.AddAuthorization();
        }

        mvcBuilder.InjectSmartCat(options);

        return mvcBuilder.Services;
    }
    private static IServiceCollection InjectSmartCat(this IMvcBuilder mvcBuilder, SmartCatOptions? options = null)
    {
        var configuration = mvcBuilder.Services.BuildServiceProvider(false).GetService<IConfiguration>();

        mvcBuilder.Services.AddAuthentication(option =>
        {
            option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
        {
            var jwtSetting = new JwtSetting();
            configuration.Bind(nameof(JwtSetting), jwtSetting);
            option.TokenValidationParameters = new TokenValidationParameters
            {
                // 签名密钥
                IssuerSigningKey = new SymmetricSecurityKey(key: jwtSetting.IssuerSigningKeyByteArray),
                ValidateIssuerSigningKey = jwtSetting.ValidateIssuerSigningKey,
                RequireExpirationTime = jwtSetting.RequireExpirationTime,
                ClockSkew = TimeSpan.FromSeconds(jwtSetting.ClockSkew),
                ValidateIssuer = jwtSetting.ValidateIssuer,
            };
        });

        mvcBuilder.Services.AddAuthorization();

        mvcBuilder.Services.AddDynamicWebApi(options.DynamicWebApiConfiguration);
        mvcBuilder.Services.AddSmartCatSwagger(options.SwaggerGenOptions);
        mvcBuilder.Services.AddSmartCatMiniProfiler();
        mvcBuilder.Services.AddEndpointsApiExplorer();
        return mvcBuilder.Services;
    }


    public static IApplicationBuilder UseSmartCat(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseSmartCatSwaggerUI();
        app.UseSmartCatMiniProfiler();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers().RequireAuthorization();
            endpoints.MapSwagger().RequireAuthorization();
        });
        return app;
    }
}
