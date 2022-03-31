using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace SmartCat.DynamicWebApi;
internal class DynamicWebApiControllerFeatureProvider : ControllerFeatureProvider
{
    private IDynamicController _dynamicController;

    public DynamicWebApiControllerFeatureProvider(IDynamicController dynamicController)
    {
        _dynamicController = dynamicController;
    }

    protected override bool IsController(TypeInfo typeInfo)
    {
        return _dynamicController.IsController(typeInfo);
    }
}
