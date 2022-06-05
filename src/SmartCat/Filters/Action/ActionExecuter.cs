using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Filter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCat.Filter.Action
{
    public static class ActionExecuter
    {
        public static ActionExecutingContext DataValidation(this ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errors = new List<string>();

                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                DefaultJsonResult result = new DefaultJsonResult()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "请求参数错误，详细信息请查看Error",
                    Error = errors
                };
                context.Result = new JsonResult(result);
            }

            return context;
        }
    }
}
