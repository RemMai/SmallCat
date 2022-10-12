using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SmallCat.ServiceAutoDiscover.Attributes;

namespace SmallCat.ServiceAutoDiscover.Extensions;

public static class ServiceAutoDiscoverExtensions
{
    /// <summary>
    /// 自动注册标识的服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServiceAutoDiscover(this IServiceCollection services)
    {
        Console.WriteLine("[ServiceAutoDiscoverService] 启动中...");
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
                    throw new Exception($"{type.Name} 自动注入失败，请检查代码");
            }

            Console.WriteLine($"'{type.Name}'注入到DI成功");
        }

        return services;
    }
}