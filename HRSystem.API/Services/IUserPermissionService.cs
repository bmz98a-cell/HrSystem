namespace HRSystem.API.Services
{
    public interface IUserPermissionService
    {
        Task AssignRoleAsync(int userId, int roleId);
        Task AssignPermissionAsync(int userId, int permissionId);
        Task<List<string>> GetUserPermissionsAsync(int userId);
    }
}
