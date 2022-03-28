using Panda.DynamicWebApi.Attributes;
using Panda.DynamicWebApi;
namespace RemMai.Center.Api.Service;

public interface ITestService
{
    Task<bool> GetData();
}



[DynamicWebApi]
public class TestService : ITestService, IDynamicWebApi
{
    public Task<bool> GetData()
    {
        return Task.FromResult(true);
    }
}
