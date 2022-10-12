using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SmallCat.Model;

namespace SmallCat.JwtAuthorization.Extensions;

public static class JwtAuthorizationExtensions
{
    public static IServiceCollection AddJwt<TAuthorizationHandler>(this IServiceCollection services)
        where TAuthorizationHandler : class, IAuthorizationHandler
    {

        // init 

        var token = new JwtSetting();
        Cat.ConfigurationManager.Bind("JwtSettings", token);
        if (token.IssuerSigningKey.Length < 64)
        {
            throw SmallCatMiao.Gugu("JwtSettings.IssuerSigningKey.length must >= 64");
        }
        Cat.JwtSetting = token;
        // Register

        services.AddSingleton<IAuthorizationPolicyProvider, DefaultAuthorizationPolicyProvider>();

        services.AddScoped<IAuthorizationHandler, TAuthorizationHandler>();

        var authenticationBuilder = services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        authenticationBuilder.AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                // 是否验证发布者
                // 发布者名称
                ValidateIssuer = token.ValidateIssuer,
                ValidIssuer = token.ValidIssuer,

                ValidAudience = token.ValidAudience,
                ValidateAudience = token.ValidateAudience,

                ValidateIssuerSigningKey = token.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(token.IssuerSigningKeyByteArray),

                ValidateLifetime = token.ValidateLifetime,
                RequireExpirationTime = token.RequireExpirationTime,

                ClockSkew = TimeSpan.FromTicks(token.ClockSkew),
            };
        });
        services.AddAuthorization();
        return services;
    }

}
