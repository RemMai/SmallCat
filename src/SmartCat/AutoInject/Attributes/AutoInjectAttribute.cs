using Microsoft.Extensions.DependencyInjection;
namespace SmartCat.AutoInject;

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
    }

    public AutoInjectAttribute(Type interfaceType, ServiceLifetime serviceLifetime)
    {
        Type = interfaceType;
        InjectType = serviceLifetime switch
        {
            ServiceLifetime.Transient => typeof(ITransient),
            ServiceLifetime.Singleton => typeof(ISingleton),
            _ => typeof(IScoped),
        };
    }

    public Type Type { get; set; }

    /// <summary>
    /// 注入类型
    /// </summary>
    public Type InjectType { get; set; } = typeof(IScoped);

    public string InjectName { get => InjectType.Name; }
}

