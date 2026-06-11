public class ApiResponse<T>
{
    public bool success { get; set; }
    public string message { get; set; } = String.Empty;
    public T? data { get; set; }
}