using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SmartCat.DynamicWebApi;
internal interface IActionRouteFactory
{
    string CreateActionRouteModel(string areaName, string controllerName, ActionModel action);
}

internal class DefaultActionRouteFactory : IActionRouteFactory
{
    private static string GetApiPreFix(ActionModel action) => AppConsts.DefaultApiPreFix;

    public string CreateActionRouteModel(string areaName, string controllerName, ActionModel action)
    {
        var apiPreFix = GetApiPreFix(action);
        var routeStr = $"{apiPreFix}/{areaName}/{controllerName}/{action.ActionName}".Replace("//", "/");
        return routeStr;
    }
}
