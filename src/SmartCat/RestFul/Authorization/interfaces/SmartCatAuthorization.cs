using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.RestFul;

public sealed class SmartCatAuthorization : IAuthorizationFactory
{
    public bool IsOk(AuthorizationFilterContext context)
    {
        return true;
    }

    public RestFulResult<object> SetErrorResult(AuthorizationFilterContext context)
    {
        return new RestFulResult<object>() { Message = "The Request NoAuthorization!", StatusCode = 401, Success = false };
    }
}
