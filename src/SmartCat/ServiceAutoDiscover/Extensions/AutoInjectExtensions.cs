using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SmartCat.ServiceAutoDiscover.Attributes;

namespace SmartCat.AutoDi.Extensions;

public static class AutoDiExtensions
{
    /// <summary>
    /// 自动注册标识的服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAutoDi(this IServiceCollection services)
    {
        var allTypes = Cat.ProjectAssemblies
            .SelectMany(assembly => assembly.GetTypes().Where(type => type.IsClass && !type.IsSealed && !type.IsAbstract && type.IsPublic && type.IsDefined(typeof(ServiceAutoDiscoverAttribute), false)))
            .ToList();

        foreach (var type in allTypes)
        {
            var attribute = type.GetCustomAttribute<ServiceAutoDiscoverAttribute>()!;

            switch (attribute.Life)
            {
                case 1:
                    services.AddScoped(attribute.ImplementationInterface, type);
                    break;
                case 2:
                    services.AddSingleton(attribute.ImplementationInterface, type);
                    break;
                case 3:
                    services.AddTransient(attribute.ImplementationInterface, type);
                    break;
                default:
                    throw new Exception($"{type.Name} ServiceAutoDiscover Error, Lift Value Error.");
            }
            
            
        }

        return services;
    }
}