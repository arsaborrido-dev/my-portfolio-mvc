namespace my_portfolio_mvc.ViewModels.Address;

public class AddressViewModel
{
    public long Id { get; set; }
    public string ZipCode { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
}
