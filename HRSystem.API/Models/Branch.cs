namespace HRSystem.API.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TenantId { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
