namespace my_portfolio_mvc.ViewModels.Skill
{
    public class SkillViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<string> Projects { get; set; } = new();
    }
}
