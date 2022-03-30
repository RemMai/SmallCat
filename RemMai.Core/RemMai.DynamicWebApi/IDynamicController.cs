using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using RemMai.Helpers;

namespace RemMai.DynamicWebApi;
public interface IDynamicController
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

        var mapRouter = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<DynamicWebApiAttribute>(typeInfo);

        if ((mapRouter != null && mapRouter.MapRouter == false) || mapRouter == null)
        {
            return false;
        }

        if (mapRouter == null || ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<NonControllerAttribute>(typeInfo) != null)
        {
            return false;
        }

        return true;
    }
}
