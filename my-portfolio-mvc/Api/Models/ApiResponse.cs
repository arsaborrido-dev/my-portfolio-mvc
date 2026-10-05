namespace my_portfolio_mvc.Api.Models;

public sealed class ApiResponse<T>
{
    public bool Succeeded { get; init; }
    public string? ErrorCode { get; init; }
    public string? ErrorMessage { get; init; }
    public T? Data { get; init; }
}
