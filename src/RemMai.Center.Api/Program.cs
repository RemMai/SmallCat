using RemMai.Center.Api.Handler;
using SmartCat;

var builder = WebApplication.CreateBuilder(args).InitSmartCat();
builder.Services.AddControllers().InjectSmartCat();
var app = builder.Build();
app.UseSmartCat();
app.Run();