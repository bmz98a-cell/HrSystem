using HRSystem.API.Models;

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public decimal Salary { get; set; }

    public int TenantId { get; set; }

    public int BranchId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Branch Branch { get; set; } = null!;
}