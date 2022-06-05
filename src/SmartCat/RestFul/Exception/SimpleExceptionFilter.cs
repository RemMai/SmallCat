using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCat.Model;

namespace SmartCat.RestFul
{
    public class SimpleExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.RestFulException();
        }

    }
}
