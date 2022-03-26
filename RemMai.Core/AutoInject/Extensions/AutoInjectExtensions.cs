using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RemMai.Inject;
namespace RemMai.Extensions;

public static class AutoInjectExtensions
{
    public static IServiceCollection AutoInjectByAttribute(this IServiceCollection services)
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        var assemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToList();
        foreach (var assembly in assemblies)
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

    public static IServiceCollection AutoInjectByInterface(this IServiceCollection services)
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        var assemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToList();

        var scope = nameof(IScope);
        var single = nameof(ISingleton);
        var transient = nameof(ITransient);

        foreach (var assembly in assemblies)
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

    public static IServiceCollection AutoInject(this IServiceCollection services)
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        var assemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToList();

        var scope = nameof(IScope);
        var single = nameof(ISingleton);
        var transient = nameof(ITransient);

        var bigTypes = assemblies.Select(e => e.GetTypes().Where(t => t.GetCustomAttribute<AutoInjectAttribute>() != null || (t.GetInterface(scope) ?? t.GetInterface(single) ?? t.GetInterface(transient)) != null)).ToList();

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
                    interfaceType = type.GetInterface($"I{type.Name}");
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
