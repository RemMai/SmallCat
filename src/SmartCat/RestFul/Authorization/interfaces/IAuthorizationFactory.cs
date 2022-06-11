using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Model;

namespace SmartCat.RestFul
{
    public interface IAuthorizationFactory
    {
        bool IsOk(AuthorizationFilterContext context);
        RestFulResult<object> SetErrorResult(AuthorizationFilterContext context);
    }
}
