using RemMai.Center.Api.Handler;
using RemMai.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.InitApp<JwtHandler>();
var app = builder.Build();

app.UseCors();
// 全局路由
app.UseRouting();
// 授权
app.UseAuthorization();
app.UseAuthentication();
// 控制器
app.MapControllers();
// 添加Swagger界面
app.UseRemMaiSwagger();
app.Run();