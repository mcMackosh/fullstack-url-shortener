public class ErrorResponse<T>
{
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public ErrorResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}