namespace my_portfolio_mvc.Api.Models.Phone;
public sealed class PhoneDto
{
    public long Id { get; init; }
    public string AreaCode { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset UpdatedAtUtc { get; init; }

}
