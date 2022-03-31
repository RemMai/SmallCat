using System;
using System.Reflection;
using SmartCat.Helpers;

namespace SmartCat.DynamicWebApi;
[Serializable]
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public class DynamicWebApiAttribute : Attribute
{
    /// <summary>
    /// Equivalent to AreaName
    /// </summary>
    public string Module { get; set; }
    /// <summary>
    /// 映射路由
    /// </summary>
    public bool MapRouter { get; set; } = true;

    /// <summary>
    /// 是否在SwaggerUI展示
    /// </summary>
    public bool Visible { get; set; } = true;

    public DynamicWebApiAttribute()
    {
        Visible = true;
        MapRouter = true;
    }

    public DynamicWebApiAttribute(bool mapRouter = true, bool visible = true)
    {
        Visible = visible;
        MapRouter = mapRouter;
    }

}
