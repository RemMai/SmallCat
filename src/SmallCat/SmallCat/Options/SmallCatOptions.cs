using SmallCat.DynamicWebApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmallCat
{
    public class SmallCatOptions
    {
        public Action<DynamicWebApiOptions>? DynamicWebApiConfiguration { get; set; } = null;
        public Action<SwaggerGenOptions>? SwaggerGenOptions { get; set; } = null;
    }
}
