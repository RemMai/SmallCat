using Microsoft.AspNetCore.Authorization;
using SmartCat.Inject;

namespace SmartCat;

public abstract class AuthorizationHandler : IAuthorizationHandler
{
    /// <summary>
    /// 验证鉴权是否成功
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual Task<bool> IsAuth(AuthorizationHandlerContext context)
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

        var isAuth = await IsAuth(context);

        if (isAuth)
        {
            context.Succeed(noAuthRequirements.First());
        }
        else context.Fail();
    }
}
