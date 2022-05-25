using SmartCat;

var builder = WebApplication.CreateBuilder(args);
builder.InitSmartCat();
var app = builder.Build();
app.UseSmartCat();
app.Run();