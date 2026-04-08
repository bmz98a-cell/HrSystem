using HRSystem.API.Models;

public class Permission
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}