using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartCat.UnifiedResponse.Exception
{
    public class SmartCatExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.UnifiedException();
        }

    }
}
