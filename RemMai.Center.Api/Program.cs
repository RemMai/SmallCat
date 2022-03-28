using RemMai.Center.Api.Handler;
using RemMai.Extensions;

var builder = WebApplication.CreateBuilder(args).GlobalObjectInject();
builder.Services.AutoInject().AddJwtAuthorization<JwtHandler>();
builder.Services.AddControllers();
builder.Services.AddDynamicWebApi();

var app = builder.Build();
app.UseCors();
app.UseAuthorization();
app.UseAuthentication();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.UseRemMaiSwagger();
app.Run();