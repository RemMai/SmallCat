using System;
using System.Reflection;
using Panda.DynamicWebApi.Helpers;

namespace Panda.DynamicWebApi.Attributes;
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
    public bool Mapping { get; set; }

    /// <summary>
    /// 是否在SwaggerUI展示
    /// </summary>
    public bool Show { get; set; }

    public DynamicWebApiAttribute(bool mapping = true, bool show = true)
    {
        Mapping = mapping;
        Show = show;
    }

    internal static bool IsExplicitlyEnabledFor(Type type)
    {
        var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicWebApiAttribute>();
        return remoteServiceAttr != null;
    }

    internal static bool IsExplicitlyDisabledFor(Type type)
    {
        var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicWebApiAttribute>();
        return remoteServiceAttr != null;
    }

    internal static bool IsMetadataExplicitlyEnabledFor(Type type)
    {
        var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicWebApiAttribute>();
        return remoteServiceAttr != null;
    }

    internal static bool IsMetadataExplicitlyDisabledFor(Type type)
    {
        var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<DynamicWebApiAttribute>();
        return remoteServiceAttr != null;
    }

    internal static bool IsMetadataExplicitlyDisabledFor(MethodInfo method)
    {
        var remoteServiceAttr = method.GetSingleAttributeOrNull<DynamicWebApiAttribute>();
        return remoteServiceAttr != null;
    }

    internal static bool IsMetadataExplicitlyEnabledFor(MethodInfo method)
    {
        var remoteServiceAttr = method.GetSingleAttributeOrNull<DynamicWebApiAttribute>();
        return remoteServiceAttr != null;
    }
}
