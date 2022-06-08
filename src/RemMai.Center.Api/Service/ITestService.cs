using CliWrap;
using Microsoft.AspNetCore.Mvc;
using SmartCat.DynamicWebApi;
using SmartCat.Model;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RemMai.Center.Api.Service;

public interface ITestService
{
    Task<string> GetService(string? eee);
    string GetData(string eee);

    Task<int> GetInt(int a);

    Task<RestFulResult<int>> GetName(RequestData name);
    Task<List<string>> GetValue(string value);
    string GetValue(ValueObject valueOjb);
}

public class ValueObject
{
    public string Value { get; set; }
}

[DynamicWebApi]
public class TestService : ITestService, IDynamicWebApi
{
    [HttpGet]
    public Task<string> GetService(string? eee)
    {
        Console.WriteLine("333");
        return Task.FromResult(eee);
    }

    public string GetData(string eee)
    {
        return eee;
    }

    public async Task<int> GetInt(int a)
    {
        return a;
    }
    [HttpPost]
    public async Task<RestFulResult<int>> GetName(RequestData name)
    {
        return new RestFulResult<int>() { Data = name.Age };
    }
    [HttpPost]
    public async Task<List<string>> GetValue(string value)
    {
        var stdOutBuffer = new StringBuilder();
        
        var cmd  = Cli.Wrap("systeminfo")
                | stdOutBuffer;
        await cmd.ExecuteAsync();

        var result = stdOutBuffer.ToString().Split("\r\n").Where(e => !string.IsNullOrEmpty(e.Trim())).ToList();

        return result;
    }

    public string GetValue(ValueObject valueOjb)
    {
        return valueOjb.Value + "--";
    }
}


public class RequestData : IValidatableObject
{
    [Required(ErrorMessage = "不可为空"), Range(1, int.MaxValue, ErrorMessage = "必须大于1")]
    public int Age { get; set; }


    [Required(ErrorMessage = "不可为空"), MinLength(1, ErrorMessage = "长度大于1")]
    public string Name { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Age != 18)
        {
            yield return new ValidationResult("年龄必须等于18岁");
        }
    }
}