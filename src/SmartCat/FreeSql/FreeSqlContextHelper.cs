using SmartCat.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using SmartCat.Model;
using System.Threading.Tasks;

namespace SmartCat.FreeSql;

public static class FreeSqlContextHelper
{
    private static List<Type> _Entities { get; set; } = new List<Type>();
    private static List<FreeSqlEntityGroup> EntityGroup { get; set; } = new List<FreeSqlEntityGroup>();


    public static List<Type> Entities
    {
        get
        {
            return _Entities ??= LoadDataEntities(reload: true);
        }
    }
    public static bool CheckDataEntity(Type type) => !type.IsGenericType && !type.IsInterface && type.IsPublic && !type.IsSealed && type.GetInterfaces().Any(face => face.IsGenericType ? face.GetGenericTypeDefinition() == typeof(IDataEntity<>) : face == typeof(IDataEntity));
    public static bool CheckDataEntity<T>(Type type) where T : IDbLocker => type.GetInterfaces().Any(face => face.IsGenericType && face.GenericTypeArguments.Any(args => args is T));
    public static List<Type> LoadDataEntities(bool reload = false)
    {
        if (reload || !_Entities.Any())
        {
            _Entities = Cat.ProjectAssemblies.SelectMany(e => e.GetTypes().Where(CheckDataEntity)).Where(type =>
                type.GetCustomAttributes(false).Any(att => att.GetType().Name == nameof(TableAttribute) && (att.GetType().GetProperty("DisableSyncStructure") == null || !(bool)(att.GetType().GetProperty("DisableSyncStructure").GetValue(att)!))
            )).ToList();
        }
        return _Entities;
    }
    public static List<Type> GetDataEntities<T>() where T : IDbLocker => _Entities.Where(CheckDataEntity<T>).ToList();

    public static void GroupDataBaseEntity()
    {

        Entities.ForEach(type =>
        {
            var faces = type.GetInterfaces().Where(face => face == typeof(IDataEntity) || face.IsGenericType && face.GenericTypeArguments.Any(args => args.IsAssignableFrom(typeof(IDbLocker))));

            type

        });

    }
}
