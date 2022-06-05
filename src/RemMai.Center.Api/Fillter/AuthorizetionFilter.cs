using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Filter.Authorization;

namespace RemMai.Center.Api.Fillter
{
    public class AuthorizetionFilter : SimpleAuthorizetionFilter
    {
        public override bool CheckAuthorization(AuthorizationFilterContext context)
        {
            return true;
        }
    }
}
