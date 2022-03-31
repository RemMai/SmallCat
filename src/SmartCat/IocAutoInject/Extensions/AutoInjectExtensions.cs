using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using SmartCat.DynamicWebApi;
using SmartCat.Inject;
namespace SmartCat.Extensions;

public static class AutoInjectExtensions
{
    /// <summary>
    /// 自动注册标识的服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AutoInject(this IServiceCollection services)
    {
        List<Type> lifeTypes = new() { typeof(IScoped), typeof(ISingleton), typeof(ITransient) };

        List<Type> allTypes = RemMaiApp.ProjectAssemblies.SelectMany(e =>
            e.GetTypes().Where(g => g.IsPublic && (g.IsDefined(typeof(AutoInjectAttribute)) || g.GetInterfaces().Intersect(lifeTypes).Any()))
        ).ToList();

        foreach (var type in allTypes)
        {
            var interfaces = type.GetInterfaces();
            AutoInjectAttribute attr = type.GetCustomAttribute<AutoInjectAttribute>();
            if (attr == null)
            {
                var _interface = interfaces.Last(e => e.IsPublic && lifeTypes.Contains(e));
                var lifeType = interfaces.LastOrDefault(e => lifeTypes.Contains(e)) ?? typeof(IScoped);
                attr = new(_interface);
                attr.InjectType = lifeType;
            }

            switch (attr.InjectName)
            {
                case nameof(IScoped):
                    services.AddScoped(attr.Type, type);
                    break;
                case nameof(ISingleton):
                    services.AddSingleton(attr.Type, type);
                    break;
                case nameof(ITransient):
                    services.AddTransient(attr.Type, type);
                    break;
                default:
                    throw new($"{type.FullName} AutoInject Error! Plase Check Code.😁by RemMai.");
            }
        }
        return services;
    }
}
