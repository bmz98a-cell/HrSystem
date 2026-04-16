using HRSystem.API.Models;
using Microsoft.EntityFrameworkCore;
using HRSystem.API.Services;

namespace HRSystem.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<License> Licenses { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Global Query Filter (مع حماية من الكراش)
            modelBuilder.Entity<Employee>()
                .HasQueryFilter(e => e.TenantId == GetCurrentTenantId());
            modelBuilder.Entity<UserRole>()
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<RolePermission>()
                .HasKey(x => new { x.RoleId, x.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasKey(x => new { x.UserId, x.PermissionId });
            modelBuilder.Entity<Branch>()
    .HasOne<Tenant>()
    .WithMany()
    .HasForeignKey(b => b.TenantId)
    .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Branch>()
    .HasQueryFilter(b => b.TenantId == GetCurrentTenantId());

            modelBuilder.Entity<RolePermission>()
    .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId });

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<Role>()
    .HasQueryFilter(r => r.TenantId == _tenantProvider.GetTenantId());

            modelBuilder.Entity<Permission>()
                .HasQueryFilter(p => p.TenantId == _tenantProvider.GetTenantId());
        }

        // 🔒 دالة آمنة بدل GetTenantId مباشرة
        private int GetCurrentTenantId()
        {
            try
            {
                return _tenantProvider.GetTenantId();
            }
            catch
            {
                return 0; // وقت التصميم أو بدون توكن
            }
        }

        // 🔥 حماية عند الحفظ (أهم شيء)
        public override int SaveChanges()
        {
            ApplyTenantSecurity();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTenantSecurity();
            return await base.SaveChangesAsync(cancellationToken);
        }

        // 🔐 فرض TenantId على كل العمليات
        private void ApplyTenantSecurity()
        {
            int tenantId;

            try
            {
                tenantId = _tenantProvider.GetTenantId();
            }
            catch
            {
                return; // لو مافي توكن (وقت التطوير)
            }

            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is ITenantEntity &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (ITenantEntity)entry.Entity;

                // 🔒 إجبار TenantId
                entity.TenantId = tenantId;
            }
        }
    }

}