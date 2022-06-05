using Microsoft.AspNetCore.Mvc;
using RemMai.Center.Api.Fillter;
using SmartCat;
using SmartCat.Filter.Action;
using SmartCat.Filter.Exception;

var builder = WebApplication.CreateBuilder(args).InitSmartCat();
builder.Services.AddControllers().InjectSmartCat(null, services =>
{
    services.Configure<MvcOptions>(options =>
    {
        options.Filters.Add<AuthorizetionFilter>();
    });
});

var app = builder.Build();
app.UseSmartCat();
app.Run();