using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Filter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.Filter.Authorize
{
    public class SimpleAuthorizeFilter : IAsyncAuthorizationFilter, IAuthorizationFilter
    {

        public virtual async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Execute(context);
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Execute(context);
        }
        public void Execute(AuthorizationFilterContext context)
        {
            if (!Authorization(context))
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(NoAuthorizationResult());
            }
        }
        public virtual bool Authorization(AuthorizationFilterContext context) => true;
        public virtual object NoAuthorizationResult() => new DefaultJsonResult { Message = "The Request NoAuthorization!", StatusCode = 401, Success = false };

    }
}
