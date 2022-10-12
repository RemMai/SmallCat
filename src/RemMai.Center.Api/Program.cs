using SmallCat;

var builder = WebApplication.CreateBuilder(args).InitSmallCat();
builder.Services.AddControllers().InjectSmallCat();
var app = builder.Build();
app.UseSmallCat();
app.Run();