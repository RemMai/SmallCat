using RemMai.Extensions;

var builder = WebApplication.CreateBuilder(args).GlobalObjectInject();

builder.Services.AddControllers();

builder.Services.AddDynamicWebApi().AutoInject();

var app = builder.Build();

app.UseCors();

app.UseRemMaiSwagger();

app.Run();
