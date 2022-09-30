using System.ComponentModel.DataAnnotations.Schema;
using SmartCat.Model;
using StackExchange.Profiling;

namespace SmartCat.FreeSqlIdleBus;

public static class FreeSqlContextHelper
{
    public static List<FreeSqlEntityGroup> EntityGroups { get; internal set; } = new List<FreeSqlEntityGroup>();
    public static List<Type> DataEntities { get; internal set; }
    // 所有的DbLocker
    public static List<Type> DbLockers { get; internal set; } = new List<Type>();

    /// <summary>
    /// 初始化
    /// </summary>
    static FreeSqlContextHelper()
    {
        // 获取全部的数据实体
        FindAllDataEntities();

        // 分类所有的数据实体到不懂的DbLock。
        VerbDataEntities();

        DbLockers = Cat.ProjectAssemblies.SelectMany(e =>
            e.GetTypes().Where(t
                => t.IsInterface
                && (t.IsAssignableFrom(typeof(IDbLocker)) || t == typeof(IDbLocker)))).ToList();
    }
    /// <summary>
    /// 查找全部的数据实体
    /// </summary>
    private static void FindAllDataEntities()
    {
        DataEntities = Cat.ProjectAssemblies.SelectMany(e => e.GetTypes().Where(CheckTypeIsDataEntity)).ToList();
    }

    /// <summary>
    /// 分类所有的数据实体
    /// </summary>
    private static void VerbDataEntities()
    {
        foreach (var type in DataEntities)
        {
            var genericInterfaces = type.GetInterfaces().Where(face => face.IsGenericType
                                                                    && face.GetInterfaces().Any(e => e == typeof(IDataEntity))
                                                                    && face.GenericTypeArguments.Any(args => args.GetInterfaces().Any(t => t == typeof(IDbLocker)))
                                                                    ).ToList();
            if (genericInterfaces.Any())
            {
                var lockers = genericInterfaces.SelectMany(e => e.GenericTypeArguments.Where(type1 => type1 == typeof(IDbLocker) || type1.GetInterfaces().Any(t => t == typeof(IDbLocker)))).ToList();
                foreach (var locker in lockers)
                {
                    Add(locker, type);
                }
            }
            else
            {
                var isCommonEntity = type.GetInterfaces().Any(face => face == typeof(IDataEntity));
                if (isCommonEntity)
                {
                    Add(typeof(IDbLocker), type);
                }
            }
        }
    }

    #region Other
    /// <summary>
    /// 判断数据对象实体是否是<see cref="IDataEntity"/>的直接、间接实现类。
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool CheckTypeIsDataEntity(Type type) => type.IsPublic
                                                        && !type.IsSealed
                                                        && !type.IsInterface
                                                        && !type.IsGenericType
                                                        && type.GetInterfaces().Any(face => face == typeof(IDataEntity)
                                                        && type.GetCustomAttributes(false).Any(att => att.GetType().Name == nameof(TableAttribute) && (att.GetType().GetProperty("DisableSyncStructure") == null || !(bool)(att.GetType().GetProperty("DisableSyncStructure").GetValue(att)!)))
                                                    );
    /// <summary>
    /// 将DataEntity加入到Entities中
    /// </summary>
    /// <param name="locker"></param>
    /// <param name="type"></param>
    private static void Add(Type locker, Type type)
    {
        var item = EntityGroups.FirstOrDefault(e => e.Locker == locker);
        if (item == null)
        {
            item = new FreeSqlEntityGroup() { Locker = locker, Entities = new List<Type> { type } };
            EntityGroups.Add(item);
        }
        else
        {
            if (item.Entities.All(e => e != type))
            {
                item.Entities.Add(type);
            }
        }
    }





    #endregion


    #region FreeSql Handler
    public static void Aop_CurdAfter(object? sender, FreeSql.Aop.CurdAfterEventArgs e)
    {
        Console.WriteLine();

        var realSql = e.Sql;

        e.DbParms.ToList().ForEach(p =>
        {
            realSql = realSql.Replace(p.ParameterName, $"'{p.Value}'");
        });

        MiniProfiler.Current.CustomTiming($"CurdAfter", realSql, executeType: "Execute FreeSQL Query", true);
    }
    #endregion
}


