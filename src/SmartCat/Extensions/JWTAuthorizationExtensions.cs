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
using SmartCat.Model;

namespace SmartCat.Extensions.Authorization;

public static class JWTAuthorizationExtensions
{
    /// <summary>
    /// 添加Jwt鉴权
    /// </summary>
    /// <param name="services">服务</param>
    /// <param name="globaAuthorization">是否开启全局授权，默认：True</param>
    /// <param name="authenticationConfigure">授权配置</param>
    /// <param name="jwtBearerConfigure">jwt配置</param>
    /// <returns></returns>
    public static IServiceCollection AddJwtAuthorization<AuthorizationHandler>(this IServiceCollection services, bool globaAuthorization = true, Action<AuthenticationOptions>? authenticationConfigure = null, Action<JwtBearerOptions>? jwtBearerConfigure = null)
        where AuthorizationHandler : class, IAuthorizationHandler
    {
        services.AddTransient<IAuthorizationHandler, AuthorizationHandler>();

        if (globaAuthorization)
        {
            services.Configure<MvcOptions>(options => options.Filters.Add(new AuthorizeFilter()));
        }

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authenticationConfigure?.Invoke(options);
        }).AddJwtBearer(options =>
        {
            var jwtSetting = new JwtSetting();
            Cat.Configuration.Bind("JwtSetting", jwtSetting);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                // 签名密钥
                IssuerSigningKey = new SymmetricSecurityKey(key: jwtSetting.IssuerSigningKeyByteArray),
                ValidateIssuerSigningKey = jwtSetting.ValidateIssuerSigningKey,
                RequireExpirationTime = jwtSetting.RequireExpirationTime,
                ClockSkew = TimeSpan.FromSeconds(jwtSetting.ClockSkew),
                ValidateIssuer = jwtSetting.ValidateIssuer,
            };
            jwtBearerConfigure?.Invoke(options);
        });
        return services;
    }


}
