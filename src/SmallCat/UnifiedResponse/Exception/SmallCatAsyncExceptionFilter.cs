using Microsoft.AspNetCore.Mvc.Filters;

namespace SmallCat.UnifiedResponse.Exception;

public class SmartCatAsyncExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        context.UnifiedException(); await Task.Run(() => { });
    }
}
