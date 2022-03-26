using RemMai.Inject;

namespace RemMai.Inject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AutoInjectAttribute : Attribute
{
    /// <summary>
    /// Scope
    /// </summary>
    /// <param name="interfaceType"></param>
    public AutoInjectAttribute(Type interfaceType)
    {
        Type = interfaceType;
        InjectType = InjectType.Scope;
    }

    public AutoInjectAttribute(Type interfaceType, InjectType injectType)
    {
        Type = interfaceType;
        InjectType = injectType;
    }

    public Type Type { get; set; }

    /// <summary>
    /// 注入类型
    /// </summary>
    public InjectType InjectType { get; set; }
}

