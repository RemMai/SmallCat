namespace SmallCat.Model;

public class UnifiedResult<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
    public T Data { get; set; }
    public object Error { get; set; }
}
