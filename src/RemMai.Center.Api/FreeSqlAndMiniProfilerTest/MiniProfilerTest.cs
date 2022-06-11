using RemMai.Center.Api.Model;
using SmartCat.DynamicWebApi;

namespace RemMai.Center.Api.FreeSqlAndMiniProfilerTest;


public interface IMiniProfilerTest
{
    Task<string> CreateUser(User user);
}
[DynamicWebApi]
public class MiniProfilerTest : IMiniProfilerTest, IDynamicWebApi
{
    public async Task<string> CreateUser(User user)
    {
        user.Id = 0;
        await MyDb.Db.Insert<User>(user).ExecuteAffrowsAsync();
        return user.Name;
    }
}
