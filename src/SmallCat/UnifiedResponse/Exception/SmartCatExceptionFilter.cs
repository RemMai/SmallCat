using Microsoft.AspNetCore.Mvc.Filters;

namespace SmallCat.UnifiedResponse.Exception
{
    public class SmartCatExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.UnifiedException();
        }

    }
}
