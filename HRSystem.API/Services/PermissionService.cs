using HRSystem.API.Data;

namespace HRSystem.API.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public PermissionService(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public async Task<Permission> CreateAsync(string name, string description)
        {
            var permission = new Permission
            {
                Name = name,
                Description = description,
                TenantId = _tenantProvider.GetTenantId()
            };

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            return permission;
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            return await _context.Permissions.ToListAsync();
        }
    }
}
