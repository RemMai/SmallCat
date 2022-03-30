using RemMai.DynamicWebApi;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RemMai.Center.Api.Service;

public interface ITestService
{
    void Get();
}

[DynamicWebApi]
public class TestService : ITestService
{
    [HttpGet]
    public void Get()
    {
        Console.WriteLine("333");
    }
}
