using SingleStage.Entities;

namespace SingleStage.Entities
{
    public partial class Employee
    {
        public string DisplayName => $"{Id}: {Username}";
    }
}
