using StackExchange.Profiling;
using RemMai.Center.Api.Model;
using SmartCat.FreeSqlExtensions;

namespace RemMai.Center.Api;

public static class MyDb
{
    public static IdleBus<IFreeSql> DleBus { get; set; } = new IdleBus<IFreeSql>(TimeSpan.FromMilliseconds(5));

    public static void InitDataBase()
    {
        FreeSqlContextHelper.Init();

        var entityGroups = FreeSqlContextHelper.EntityGroups;

        List<FreeSqlDbConfiguration> dbConfigurations = new()
        {
            new FreeSqlDbConfiguration() { Locker = typeof(IMaiDbLocker) , ConnectionString = "Data Source=RemMai.db;Pooling=true;Min Pool Size=1" , FreeSqlDataType = FreeSql.DataType.Sqlite }
        };

        dbConfigurations.ForEach(configuration =>
        {
            var item = entityGroups.FirstOrDefault(e => e.Locker == configuration.Locker);

            if (item != null)
            {
                DleBus.Register(item.Locker.Name, () =>
                {
                    var Db = new FreeSql.FreeSqlBuilder()
                     .UseConnectionString(configuration.FreeSqlDataType, configuration.ConnectionString)
                     .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
                     .Build();

                    if (SmartCat.Cat.Environment.IsDevelopment())
                    {
                        Db.CodeFirst.IsAutoSyncStructure = true;
                        Db.CodeFirst.SyncStructure(item.Entities.ToArray());
                    }
                    Db.Aop.CurdAfter += FreeSqlContextHelper.Aop_CurdAfter;

                    return Db;
                });
            }
        });
    }

}
