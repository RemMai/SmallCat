using Panda.DynamicWebApi.Attributes;
using Panda.DynamicWebApi;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RemMai.Center.Api.Service;

public interface ITestService
{
    Task<bool> GetData();
}

[DynamicWebApi]
public class TestService<T> : ITestService where T : class
{
    public Task<bool> GetData() 
    {
        return Task.FromResult(true);
    }
}
