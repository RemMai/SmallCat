using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SmallCat.FreeSqlIdleBus.Extensions;

/// <summary>
/// FreeSql 拓展
/// </summary>
public static class FreeSqlExtensions
{
    /// <summary>
    /// Use Dynamic WebApi to Configure
    /// </summary>
    /// <param name="services">Service</param>
    /// <param name="freeSqlDbConfigurations">FreeSQL Config</param>
    /// <returns></returns>
    public static IServiceCollection AddFreeIdleBus(this IServiceCollection services, List<FreeSqlDbConfiguration> freeSqlDbConfigurations = null)
    {
        if (!freeSqlDbConfigurations.Any()) return services;
        // 初始化帮助FreeSql帮助类
        var entityGroups = FreeSqlContextHelper.EntityGroups;

        foreach (var freeSqlDbConfiguration in freeSqlDbConfigurations)
        {
            var item = entityGroups.FirstOrDefault(e => e.Locker == freeSqlDbConfiguration.Locker);
            var name = freeSqlDbConfiguration.Locker.Name;
            Cat.Db.Register(name, () =>
            {
                var db = new FreeSql.FreeSqlBuilder()
                    .UseConnectionString(freeSqlDbConfiguration.FreeSqlDataType, freeSqlDbConfiguration.ConnectionString)
                    .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
                    .Build();


                if (Cat.Environment.IsDevelopment() || freeSqlDbConfiguration.AutoSyncStructure)
                {
                    db.CodeFirst.IsAutoSyncStructure = true;
                    db.CodeFirst.SyncStructure(item.Entities.ToArray());
                }

                db.Aop.CurdAfter += FreeSqlContextHelper.Aop_CurdAfter;

                return db;
            });
        }
        return services;
    }

    public static IServiceCollection AddFreeIdleBus(this IServiceCollection services, string freeSqlConnectionString)
    {
        if (freeSqlConnectionString == null) throw new ArgumentNullException(nameof(freeSqlConnectionString));
        var freeSqlConnectionStrings = new List<FreeSqlConnectionString>();

        Cat.Configuration.Bind(freeSqlConnectionString, freeSqlConnectionStrings);

        foreach (var freeSqlDbConfiguration in freeSqlConnectionStrings)
        {
            var locker =
                FreeSqlContextHelper.DbLockers.FirstOrDefault(e => e.Name == freeSqlDbConfiguration.Locker);
            if (locker == null)
            {
                throw new NullReferenceException(
                    $"Not Found \"{freeSqlDbConfiguration.Locker}\",Please check freeSqlConnectionString item \"{freeSqlConnectionString}\".");
            }

            var item = FreeSqlContextHelper.EntityGroups.FirstOrDefault(e => e.Locker == locker);
            var name = item.Locker.Name;
            Cat.Db.Register(name, () =>
            {
                var db = new FreeSql.FreeSqlBuilder()
                    .UseConnectionString((FreeSql.DataType)freeSqlDbConfiguration.FreeSqlDataType,
                        freeSqlDbConfiguration.ConnectionString)
                    .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
                    .Build();


                if (Cat.Environment.IsDevelopment() || freeSqlDbConfiguration.AutoSyncStructure)
                {
                    db.CodeFirst.IsAutoSyncStructure = true;
                    db.CodeFirst.SyncStructure(item.Entities.ToArray());
                }

                db.Aop.CurdAfter += FreeSqlContextHelper.Aop_CurdAfter;

                return db;
            });
        }

        return services;

    }


}