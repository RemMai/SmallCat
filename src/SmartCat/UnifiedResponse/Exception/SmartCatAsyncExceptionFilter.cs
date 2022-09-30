using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartCat.UnifiedResponse.Exception;

public class SmartCatAsyncExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        context.UnifiedException(); await Task.Run(() => { });
    }
}
