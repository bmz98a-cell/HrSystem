using HRSystem.API.Data;
using HRSystem.API.Models;

namespace HRSystem.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public RoleService(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public async Task<Role> CreateAsync(string name)
        {
            var role = new Role
            {
                Name = name,
                TenantId = _tenantProvider.GetTenantId()
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return role;
        }

        public async Task AssignPermissionsAsync(int roleId, List<int> permissionIds)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
                throw new Exception("Role not found");

            // حذف القديم
            _context.RolePermissions.RemoveRange(role.RolePermissions);

            // إضافة الجديد
            var rolePermissions = permissionIds.Select(pid => new RolePermission
            {
                RoleId = roleId,
                PermissionId = pid
            });

            await _context.RolePermissions.AddRangeAsync(rolePermissions);
            await _context.SaveChangesAsync();
        }
    }
}
