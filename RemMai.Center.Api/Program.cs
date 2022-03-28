using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using RemMai.Extensions;

var builder = WebApplication.CreateBuilder(args).GlobalObjectInject();

builder.Services.AutoInject().AddJwtAuthorization<>();

builder.Services.AddControllers();

builder.Services.AddDynamicWebApi();

var app = builder.Build();

app.UseCors();

app.UseAuthorization();

app.UseRemMaiSwagger();

app.UseAuthentication();

app.Run();