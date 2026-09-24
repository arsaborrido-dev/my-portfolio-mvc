namespace my_portfolio_mvc.Api.Models;
public class SkillDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<string> Projects { get; set; } = new();
}
