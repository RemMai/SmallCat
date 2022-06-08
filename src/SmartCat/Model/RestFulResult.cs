namespace SmartCat.Model;

public class RestFulResult<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
    public T Data { get; set; }
    public object Error { get; set; }
}
