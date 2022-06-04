using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SmartCat.Filter.Authorize;
using SmartCat.Model;

namespace SmartCat.Extensions.Authorization;

public static class JWTAuthorizationExtensions
{
    /// <summary>
    /// 添加Jwt鉴权
    /// </summary>
    /// <param name="services">服务</param>
    /// <param name="authenticationConfigure">授权配置</param>
    /// <param name="jwtBearerConfigure">jwt配置</param>
    /// <returns></returns>
    public static IServiceCollection AddJwtAuthorization<AuthorizationHandler>(this IServiceCollection services, Action<AuthenticationOptions>? authenticationConfigure = null, Action<JwtBearerOptions>? jwtBearerConfigure = null) where AuthorizationHandler : class, IAuthorizationHandler
    {
        var configuration = services.BuildServiceProvider(false).GetService<IConfiguration>();

        services.AddTransient<IAuthorizationHandler, AuthorizationHandler>();

        return services;
    }

}
