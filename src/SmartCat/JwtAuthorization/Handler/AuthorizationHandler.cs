using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartCat;

[Obsolete]
public abstract class AuthorizationHandler :  IAuthorizationHandler, IFilterMetadata
{
    /// <summary>
    /// 验证鉴权是否成功
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [Obsolete]
    public virtual Task<bool> CheckAuthorization(AuthorizationHandlerContext context)
    {
        return Task.FromResult(true);
    }
    /// <summary>
    /// 实现授权接口
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>

    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        var noAuthRequirements = context.PendingRequirements;

        var authorization = await CheckAuthorization(context);

        if (authorization)
        {
            context.Succeed(noAuthRequirements.First());
        }
        else context.Fail();
    }
}
