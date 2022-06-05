using SmartCat.DynamicWebApi;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SmartCat;

namespace RemMai.Center.Api.Service;

public interface ITestService
{
    Task<string> GetService(string? eee);
    string GetData(string eee);

    Task<int> GetInt(int a);

    Task<int> GetName(RequestData name);
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
    [HttpPost, NonUnifiedResult(false)]
    public async Task<int> GetName(RequestData name)
    {
        return name.Age;
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