namespace HRSystem.API.Services
{
    public interface IPermissionService
    {
        Task<Permission> CreateAsync(string name, string description);
        Task<List<Permission>> GetAllAsync();
    }
}
