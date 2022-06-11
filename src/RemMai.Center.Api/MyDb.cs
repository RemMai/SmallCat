using StackExchange.Profiling;
using System.Reflection;
using SmartCat.DataBase;
using RemMai.Center.Api.Model;

namespace RemMai.Center.Api;

public static class MyDb
{
    public static IdleBus<IFreeSql> DataBases { get; set; } = new IdleBus<IFreeSql>(TimeSpan.FromMilliseconds(5));

    public static bool InitBaseDb()
    {
        SmartCat.Cat.Configuration.;
    }


    public static IFreeSql SetDataBase(string connectionString)
    {





        FreeSqlManager.LoadDataEntities();

        var types = FreeSqlManager.GetDataEntities<IMaiDbLocker>().ToArray();

        var Db = new FreeSql.FreeSqlBuilder()
             .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=RemMai.db;Pooling=true;Min Pool Size=1")
             .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
             .Build();

        Db.CodeFirst.IsAutoSyncStructure = true;
        Db.CodeFirst.SyncStructure(types);
        Db.Aop.CurdAfter += Aop_CurdAfter;

        return Db;
    }

    private static void Aop_CurdAfter(object? sender, FreeSql.Aop.CurdAfterEventArgs e)
    {
        var realSQL = e.Sql;

        e.DbParms.ToList().ForEach(p =>
        {
            realSQL = realSQL.Replace(p.ParameterName, $"'{p.Value}'");
        });

        MiniProfiler.Current.CustomTiming($"CurdAfter", realSQL, executeType: "Execute FreeSQL Query", true);
    }


    public static bool RegisterDatabase(string key, string connectionString, bool isReRegister = false)
    {
        if (DataBases.Exists(key))
        {
            DataBases.TryRemove(key);
            DataBases.Register(key, () => SetDataBase(connectionString));
        }

        return true;
    }




}
