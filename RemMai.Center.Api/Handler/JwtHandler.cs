using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RemMai.Center.Api.Handler;

public class JwtHandler : AuthorizationHandler
{
    public override Task<bool> IsAuth(AuthorizationHandlerContext context)
    {

    }
}
