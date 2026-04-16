namespace HRSystem.API.Services
{
    public interface IRoleService
    {
        Task<Role> CreateAsync(string name);
        Task AssignPermissionsAsync(int roleId, List<int> permissionIds);
    }
}
