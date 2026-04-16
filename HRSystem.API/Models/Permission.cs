using HRSystem.API.Models;

public class Permission
{
    public int Id { get; set; }

    public string Name { get; set; } = null!; // Example: CreateEmployee

    public string Description { get; set; } = null!;

    public int TenantId { get; set; }
}