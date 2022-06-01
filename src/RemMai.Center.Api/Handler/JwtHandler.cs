using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat;

namespace RemMai.Center.Api.Handler;

//public class JwtHandler : AuthorizationHandler
//{
//    public override Task<bool> CheckAuthorization(AuthorizationHandlerContext context)
//    {
//        return Task.FromResult(false);
//    }
//}

public class JwtHandler : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        throw new NotImplementedException();
    }
}