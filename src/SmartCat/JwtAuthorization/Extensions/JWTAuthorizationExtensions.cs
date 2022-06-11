using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartCat.Extensions.Authorization;

[Obsolete]
public static class JWTAuthorizationExtensions
{
    /// <summary>
    /// 添加Jwt鉴权
    /// </summary>
    /// <param name="services">服务</param>
    /// <param name="authenticationConfigure">授权配置</param>
    /// <param name="jwtBearerConfigure">jwt配置</param>
    /// <returns></returns>
    [Obsolete]
    public static IServiceCollection AddJwtAuthorization<AuthorizationHandler>(this IServiceCollection services, Action<AuthenticationOptions>? authenticationConfigure = null, Action<JwtBearerOptions>? jwtBearerConfigure = null) where AuthorizationHandler : class, IAuthorizationHandler
    {
        var configuration = services.BuildServiceProvider(false).GetService<IConfiguration>();

        services.AddTransient<IAuthorizationHandler, AuthorizationHandler>();

        return services;
    }

}
