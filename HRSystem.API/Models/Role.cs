using HRSystem.API.Models;

public class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!; // Admin, HR, Manager

    public int TenantId { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}