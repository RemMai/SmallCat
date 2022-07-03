using SmartCat.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using SmartCat.Model;
using System.Threading.Tasks;
using FreeSql;
using StackExchange.Profiling;

namespace SmartCat.FreeSqlExtensions;

public static class FreeSqlContextHelper
{
    public static List<FreeSqlEntityGroup> EntityGroups { get; internal set; }
    public static List<Type> DataEntities { get; internal set; }


    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        EntityGroups = new List<FreeSqlEntityGroup>();

        FindAllDataEntities();

        VerbDataEntities();
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
            var Genericinterfaces = type.GetInterfaces().Where(face => face.IsGenericType && face.GetInterfaces().Any(e => e == typeof(IDataEntity)) && face.GenericTypeArguments.Any(args => args.GetInterfaces().Any(t => t == typeof(IDbLocker)))).ToList();
            if (Genericinterfaces.Any())
            {
                var lockers = Genericinterfaces.SelectMany(e => e.GenericTypeArguments.Where(e => e == typeof(IDbLocker) || e.GetInterfaces().Any(t => t == typeof(IDbLocker)))).ToList();
                foreach (var locker in lockers)
                {
                    Add(locker, type);
                }
            }
            else
            {
                bool isCommonEntity = type.GetInterfaces().Any(face => face == typeof(IDataEntity));
                if (isCommonEntity)
                {
                    Add(typeof(IDbLocker), type);
                }
            }
        }
    }

    #region Other
    /// <summary>
    /// 判断提示是否是<see cref="IDataEntity"/>的直接、间接实现类。
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
    /// <param name="_locker"></param>
    /// <param name="_type"></param>
    private static void Add(Type _locker, Type _type)
    {
        var item = EntityGroups.FirstOrDefault(e => e.Locker == _locker);
        if (item == null)
        {
            item = new FreeSqlEntityGroup() { Locker = _locker, Entities = new List<Type>() };
            item.Entities.Add(_type);
            EntityGroups.Add(item);
        }
        else
        {
            if (!item.Entities.Any(e => e == _type))
            {
                item.Entities.Add(_type);
            }
        }
    }
    #endregion


    #region FreeSql Handler
    public static void Aop_CurdAfter(object? sender, FreeSql.Aop.CurdAfterEventArgs e)
    {
        var realSQL = e.Sql;

        e.DbParms.ToList().ForEach(p =>
        {
            realSQL = realSQL.Replace(p.ParameterName, $"'{p.Value}'");
        });

        MiniProfiler.Current.CustomTiming($"CurdAfter", realSQL, executeType: "Execute FreeSQL Query", true);
    }
    #endregion
}
