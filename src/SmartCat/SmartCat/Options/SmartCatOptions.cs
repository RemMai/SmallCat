using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SmartCat.DynamicWebApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartCat
{
    public class SmartCatOptions
    {
        public Action<DynamicWebApiOptions>? DynamicWebApiConfiguration { get; set; } = null;
        public Action<SwaggerGenOptions>? SwaggerGenOptions { get; set; } = null;
        /// <summary>
        /// 授权配置
        /// </summary>
        public Action<AuthenticationOptions>? AuthenticationConfigure = null;
        /// <summary>
        /// Jwt请求头配置
        /// </summary>
        public Action<JwtBearerOptions>? JwtBearerConfigure = null;
    }
}
