using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using SmartCat.DynamicWebApi.Helpers;

namespace SmartCat.DynamicWebApi;
internal interface IDynamicController
{
    bool IsController(Type type);
}

internal class DefaultDynamicController : IDynamicController
{
    public bool IsController(Type type)
    {
        var typeInfo = type.GetTypeInfo();

        if (!typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType)
        {
            return false;
        }

        var dynamicWebApiAttribute = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<DynamicWebApiAttribute>(typeInfo);

        if ((dynamicWebApiAttribute != null && dynamicWebApiAttribute.MapRouter == false) || dynamicWebApiAttribute == null)
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
