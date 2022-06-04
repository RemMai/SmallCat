using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Filter.Authorize;

namespace RemMai.Center.Api.Fillter
{
    public class AuthorizetionFilter : SimpleAuthorizeFilter
    {
        public override bool Authorization(AuthorizationFilterContext context)
        {
            return true;
        }
    }
}
