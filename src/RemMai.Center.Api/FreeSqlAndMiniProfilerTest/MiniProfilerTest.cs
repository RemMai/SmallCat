using RemMai.Center.Api.Model;
using SmartCat.DynamicWebApi;
using SmartCat.FreeSql;

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
        MyDb.SetDataBase("");
        var GroupEntities = FreeSqlContextHelper.GetGroupEntities();

        foreach (var item in GroupEntities)
        {
            var name1 = typeof(IDataEntity<,>).Name;
            var name2 = typeof(IDataEntity<>).Name;
            var name3 = typeof(IDataEntity).Name;

            Console.WriteLine(name1);
            Console.WriteLine(name2);
            Console.WriteLine(name3);

            Console.Write(item.Locker.Name);

            item.Entities.ForEach(e =>
            {
                Console.Write("   " + e.Name);
            });

            Console.WriteLine();
        }
        return user.Name;
    }
}
