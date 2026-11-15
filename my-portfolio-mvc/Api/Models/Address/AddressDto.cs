namespace my_portfolio_mvc.Api.Models.Address;

public sealed class AddressDto
{
    public long Id { get; init; }
    public string ZipCode { get; init; } = string.Empty;
    public string AddressLine1 { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset UpdatedAtUtc { get; init; }
}
