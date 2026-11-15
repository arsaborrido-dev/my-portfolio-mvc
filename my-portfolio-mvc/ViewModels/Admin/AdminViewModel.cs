using my_portfolio_mvc.ViewModels.Address;
using my_portfolio_mvc.ViewModels.Phone;
using my_portfolio_mvc.ViewModels.Project;

namespace my_portfolio_mvc.ViewModels.Admin;

public class AdminViewModel
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<ProjectViewModel> Projects { get; set; } = new();
    public List<AddressViewModel> Addresses { get; set; } = new();
    public List<PhoneViewModel> Phones { get; set; } = new();
}
