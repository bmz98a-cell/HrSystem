using HRSystem.API.Data;
using HRSystem.API.Models;

namespace HRSystem.API.Services
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly ApplicationDbContext _context;

        public UserPermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AssignRoleAsync(int userId, int roleId)
        {
            var exists = await _context.UserRoles
                .AnyAsync(x => x.UserId == userId && x.RoleId == roleId);

            if (!exists)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignPermissionAsync(int userId, int permissionId)
        {
            var exists = await _context.UserPermissions
                .AnyAsync(x => x.UserId == userId && x.PermissionId == permissionId);

            if (!exists)
            {
                _context.UserPermissions.Add(new UserPermission
                {
                    UserId = userId,
                    PermissionId = permissionId
                });

                await _context.SaveChangesAsync();
            }
        }

        // 🔥 أهم دالة في النظام كله
        public async Task<List<string>> GetUserPermissionsAsync(int userId)
        {
            var rolePermissions = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();

            var directPermissions = await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.Permission.Name)
                .ToListAsync();

            return rolePermissions
                .Union(directPermissions)
                .Distinct()
                .ToList();
        }
    }
}
