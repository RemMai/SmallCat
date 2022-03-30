using RemMai.Center.Api.Handler;
using RemMai.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.InitApp<JwtHandler>();
var app = builder.Build();

app.UseCors();
// ȫ��·��
app.UseRouting();
// ��Ȩ
app.UseAuthorization();
app.UseAuthentication();
// ������
app.MapControllers();
// ���Swagger����
app.UseRemMaiSwagger();
app.Run();