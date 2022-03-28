using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using RemMai.Inject;
namespace RemMai.Extensions;

public static class AutoInjectExtensions
{
    /// <summary>
    /// 仅仅根据属性注册服务生命周期
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IServiceCollection AutoInjectByAttribute(this IServiceCollection services)
    {
        foreach (var assembly in RemMaiApp.ProjectAssemblies)
        {
            var types = assembly.GetTypes().Where(a => a.GetCustomAttribute<AutoInjectAttribute>() != null).ToList();
            if (types.Count <= 0) continue;
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<AutoInjectAttribute>();
                if (attr?.Type == null) continue;
                switch (attr.InjectType)
                {
                    case InjectType.Scope:
                        services.AddScoped(attr.Type, type);
                        break;
                    case InjectType.Singleton:
                        services.AddSingleton(attr.Type, type);
                        break;
                    case InjectType.Transient:
                        services.AddTransient(attr.Type, type);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        return services;
    }

    /// <summary>
    /// 仅仅根据接口注册生命周期
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IServiceCollection AutoInjectByInterface(this IServiceCollection services)
    {
        List<Type> lifeTypes = new() { typeof(IScoped), typeof(ISingleton), typeof(ITransient) };

        List<Type> allTypes = RemMaiApp.ProjectAssemblies.SelectMany(e =>
            e.GetTypes().Where(g => g.IsPublic && (g.IsDefined(typeof(AutoInjectAttribute)) || g.GetInterfaces().Intersect(lifeTypes).Any()))

        ).ToList();

        foreach (var assembly in RemMaiApp.ProjectAssemblies)
        {
            foreach (var type in assembly.GetTypes())
            {





                var injectType = type.GetInterface(scope) ?? type.GetInterface(single) ?? type.GetInterface(transient);
                if (injectType == null) continue;
                var interfaceType = type.GetInterface($"I{type.Name}");
                if (interfaceType == null) continue;

                switch (injectType.Name)
                {
                    case nameof(IScope):
                        services.AddScoped(interfaceType, type);
                        break;
                    case nameof(ISingleton):
                        services.AddSingleton(interfaceType, type);
                        break;
                    case nameof(ITransient):
                        services.AddTransient(interfaceType, type);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        return services;
    }

    /// <summary>
    /// 根据属性或接口注册生命周期
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AutoInject(this IServiceCollection services)
    {
        var scope = nameof(IScoped);
        var single = nameof(ISingleton);
        var transient = nameof(ITransient);

        var bigTypes = RemMaiApp.ProjectAssemblies.Select(e => e.GetTypes().Where(t => t.GetCustomAttribute<AutoInjectAttribute>() != null || (t.GetInterface(scope) ?? t.GetInterface(single) ?? t.GetInterface(transient)) != null)).ToList();

        bigTypes.ForEach(types =>
        {
            foreach (var type in types)
            {
                var injectName = string.Empty;
                Type interfaceType = null;
                if (type.GetCustomAttribute<AutoInjectAttribute>() != null)
                {
                    var attr = type.GetCustomAttribute<AutoInjectAttribute>();
                    injectName = attr.InjectType.ToString().ToLower();
                    interfaceType = attr.Type;
                }
                else
                {
                    var _interface = type.GetInterface(scope) ?? type.GetInterface(single) ?? type.GetInterface(transient);


                    // 排除无关的接口

                    var interfaceTypes = type.GetInterfaces().Where(e =>
                        e != typeof(Panda.DynamicWebApi.IDynamicWebApi) // 排除动态API接口
                        e
                    )


                    interfaceType = .LastOrDefault();
                    injectName = _interface.Name.ToLower();
                }
                if (interfaceType == null) continue;

                if (injectName.Contains("scope"))
                {
                    services.AddScoped(interfaceType, type);
                }
                else if (injectName.Contains("singleton"))
                {
                    services.AddSingleton(interfaceType, type);
                }
                else if (injectName.Contains("transient"))
                {
                    services.AddTransient(interfaceType, type);
                }
            }
        });
        return services;
    }
}
