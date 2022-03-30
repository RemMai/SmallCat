using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RemMai.Extensions;

public static class GlobalObjectInjectExtensions
{
    public static WebApplicationBuilder InitApp(this WebApplicationBuilder appBuilder)
    {
        // 注册Configuration
        RemMaiApp.ConfigurationManager = appBuilder.Configuration;

        // 注册Environment
        RemMaiApp.Environment = appBuilder.Environment;

        // 注册服务集合
        RemMaiApp.Services = appBuilder.Services;

        // 注册服务提供器
        RemMaiApp.ServiceProvider = appBuilder.Services.BuildServiceProvider(false);

        // 注册全局请求上下文
        appBuilder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        RemMaiApp.HttpContext = appBuilder.Services.BuildServiceProvider(false).GetService<IHttpContextAccessor>().HttpContext;



        appBuilder.AutoScanConfigurationFile();
        appBuilder.Services.AddControllers().AddDynamicWebApiAndSwaggerGen();
        appBuilder.Services.AutoInject();

        return appBuilder;
    }
    public static WebApplicationBuilder InitApp<THandler>(this WebApplicationBuilder appBuilder) where THandler : class, IAuthorizationHandler
    {
        appBuilder.InitApp();
        appBuilder.Services.AddJwtAuthorization<THandler>();
        return appBuilder;
    }
}
