using System.Reflection;
using System.Runtime.CompilerServices;

namespace SmallCat.Helpers;

internal static class ReflectionHelper
{
    /// <summary>
    /// 获取第一个标记属性 / 空（完整搜索）
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="info"></param>
    /// <returns></returns>
    public static TAttribute GetSingleAttributeOrDefaultByFullSearch<TAttribute>(this TypeInfo info) where TAttribute : Attribute
    {
        var attributeType = typeof(TAttribute);
        if (info.IsDefined(attributeType, true))
        {
            return info.GetCustomAttributes(attributeType, true).Cast<TAttribute>().First();
        }
        else
        {
            foreach (var implInter in info.ImplementedInterfaces)
            {
                var res = GetSingleAttributeOrDefaultByFullSearch<TAttribute>(implInter.GetTypeInfo());

                if (res != null)
                {
                    return res;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 获取第一个标记属性 / 空
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="memberInfo"></param>
    /// <param name="defaultValue"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static TAttribute GetSingleAttributeOrDefault<TAttribute>(this MemberInfo memberInfo, TAttribute defaultValue = default(TAttribute), bool inherit = true) where TAttribute : Attribute
    {
        var attributeType = typeof(TAttribute);
        return memberInfo.IsDefined(typeof(TAttribute), inherit) ? memberInfo.GetCustomAttributes(attributeType, inherit).Cast<TAttribute>().First() : defaultValue;
    }


    /// <summary>
    /// 获取 MemberInfo 的第一个属性或者空
    /// </summary>
    /// <typeparam name="TAttribute">Type of the attribute</typeparam>
    /// <param name="memberInfo">The member that will be checked for the attribute</param>
    /// <param name="inherit">Include inherited attributes</param>
    /// <returns>Returns the attribute object if found. Returns null if not found.</returns>
    public static TAttribute GetSingleAttributeOrNull<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
        where TAttribute : Attribute
    {
        if (memberInfo == null)
        {
            throw new ArgumentNullException(nameof(memberInfo));
        }

        var attrs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).ToArray();
        if (attrs.Length > 0)
        {
            return (TAttribute)attrs[0];
        }

        return default(TAttribute);
    }

    /// <summary>
    /// 获取 Type 的第一个属性或者空
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="type"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static TAttribute GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(this Type type, bool inherit = true)
        where TAttribute : Attribute
    {
        var attr = type.GetTypeInfo().GetSingleAttributeOrNull<TAttribute>();
        if (attr != null)
        {
            return attr;
        }

        if (type.GetTypeInfo().BaseType == null)
        {
            return null;
        }

        return type.GetTypeInfo().BaseType.GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(inherit);
    }

    /// <summary>
    /// 判断方法是否是异步方法
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public static bool IsAsyncMethod(this MethodInfo method) => method.GetCustomAttribute<AsyncMethodBuilderAttribute>() != null || method.ReturnType.ToString().StartsWith(typeof(Task).FullName);
    /// <summary>
    /// 获取类型Task&lt;Type&gt;真实的类型
    /// <para></para>
    /// 如果被Task包裹，则返回包裹的类型，如果没被包裹则返回其本身，如果单独为Task，则返回typeof(void)
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public static Type GetRealType(this MethodInfo method) => method.IsAsyncMethod() ? (method.ReturnType.GenericTypeArguments.FirstOrDefault() ?? typeof(void)) : method.ReturnType;


    /// <summary>
    /// 转载于  https://blog.csdn.net/WPwalter/article/details/82859267
    /// <para></para>
    /// 判断指定的类型 <paramref name="type"/> 是否是指定泛型类型的子类型，或实现了指定泛型接口。
    /// </summary>
    /// <param name="type">需要测试的类型。</param>
    /// <param name="generic">泛型接口类型，传入 typeof(IXxx&lt;&gt;)</param>
    /// <returns>如果是泛型接口的子类型，则返回 true，否则返回 false。</returns>
    public static bool HasImplementedRawGeneric(this Type type, Type generic)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (generic == null) throw new ArgumentNullException(nameof(generic));

        // 测试接口。
        var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
        if (isTheRawGenericType) return true;

        // 测试类型。
        while (type != null && type != typeof(object))
        {
            isTheRawGenericType = IsTheRawGenericType(type);
            if (isTheRawGenericType) return true;
            type = type.BaseType;
        }

        // 没有找到任何匹配的接口或类型。
        return false;

        // 测试某个类型是否是指定的原始接口。
        bool IsTheRawGenericType(Type test)
            => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
    }
}
