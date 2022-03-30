using Panda.DynamicWebApi.Attributes;
using Panda.DynamicWebApi.Helpers;
using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Panda.DynamicWebApi;
public interface ISelectController
{
    bool IsController(Type type);
}

internal class DefaultSelectController : ISelectController
{
    public bool IsController(Type type)
    {
        var typeInfo = type.GetTypeInfo();

        if (!typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType)
        {
            return false;
        }


        var attr = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<DynamicWebApiAttribute>(typeInfo);

        if (attr == null)
        {
            return false;
        }

        if (ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<NonControllerAttribute>(typeInfo) != null)
        {
            return false;
        }

        return true;
    }
}
