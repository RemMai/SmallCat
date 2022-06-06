using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SmartCat.Helpers;

internal static class ReflectionHelper
{
    /// <summary>
    /// ��ȡ��һ��������� / �գ�����������
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
    /// ��ȡ��һ��������� / ��
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="memberInfo"></param>
    /// <param name="defaultValue"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static TAttribute GetSingleAttributeOrDefault<TAttribute>(this MemberInfo memberInfo, TAttribute defaultValue = default(TAttribute), bool inherit = true) where TAttribute : Attribute
    {
        var attributeType = typeof(TAttribute);
        if (memberInfo.IsDefined(typeof(TAttribute), inherit))
        {
            return memberInfo.GetCustomAttributes(attributeType, inherit).Cast<TAttribute>().First();
        }
        return defaultValue;
    }


    /// <summary>
    /// ��ȡ MemberInfo �ĵ�һ�����Ի��߿�
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
    /// ��ȡ Type �ĵ�һ�����Ի��߿�
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
    /// �жϷ����Ƿ����첽����
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public static bool IsAsyncMethod(this MethodInfo method) => method.GetCustomAttribute<AsyncMethodBuilderAttribute>() != null || method.ReturnType.ToString().StartsWith(typeof(Task).FullName);
    /// <summary>
    /// ��ȡ����Task&lt;Type&gt;��ʵ������
    /// <para></para>
    /// �����Task�������򷵻ذ��������ͣ����û�������򷵻��䱾�����������ΪTask���򷵻�typeof(void)
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public static Type GetRealType(this MethodInfo method) => method.IsAsyncMethod() ? (method.ReturnType.GenericTypeArguments.FirstOrDefault() ?? typeof(void)) : method.ReturnType;


    /// <summary>
    /// ת����  https://blog.csdn.net/WPwalter/article/details/82859267
    /// <para></para>
    /// �ж�ָ�������� <paramref name="type"/> �Ƿ���ָ���������͵������ͣ���ʵ����ָ�����ͽӿڡ�
    /// </summary>
    /// <param name="type">��Ҫ���Ե����͡�</param>
    /// <param name="generic">���ͽӿ����ͣ����� typeof(IXxx&lt;&gt;)</param>
    /// <returns>����Ƿ��ͽӿڵ������ͣ��򷵻� true�����򷵻� false��</returns>
    public static bool HasImplementedRawGeneric(this Type type, Type generic)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (generic == null) throw new ArgumentNullException(nameof(generic));

        // ���Խӿڡ�
        var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
        if (isTheRawGenericType) return true;

        // �������͡�
        while (type != null && type != typeof(object))
        {
            isTheRawGenericType = IsTheRawGenericType(type);
            if (isTheRawGenericType) return true;
            type = type.BaseType;
        }

        // û���ҵ��κ�ƥ��Ľӿڻ����͡�
        return false;

        // ����ĳ�������Ƿ���ָ����ԭʼ�ӿڡ�
        bool IsTheRawGenericType(Type test)
            => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
    }
}