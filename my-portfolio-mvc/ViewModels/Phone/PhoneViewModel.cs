namespace my_portfolio_mvc.ViewModels.Phone;

public sealed class PhoneViewModel
{
    public long Id { get; set; }
    public string AreaCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }

}
