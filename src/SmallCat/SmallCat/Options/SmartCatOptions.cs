using SmallCat.DynamicWebApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmallCat
{
    public class SmartCatOptions
    {
        public Action<DynamicWebApiOptions>? DynamicWebApiConfiguration { get; set; } = null;
        public Action<SwaggerGenOptions>? SwaggerGenOptions { get; set; } = null;
    }
}
