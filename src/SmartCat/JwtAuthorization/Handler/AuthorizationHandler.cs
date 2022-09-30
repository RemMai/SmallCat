using Microsoft.AspNetCore.Authorization;

namespace SmartCat.JwtAuthorization.Handler;

public abstract class AuthorizationHandler : IAuthorizationHandler
{
    /// <summary>
    /// 验证鉴权是否成功
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
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
            context.Succeed(noAuthRequirements.FirstOrDefault());
        }
        else
        {
            context.Fail();
        }
    }
}