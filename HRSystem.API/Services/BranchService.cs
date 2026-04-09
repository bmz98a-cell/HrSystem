using HRSystem.API.Data;
using HRSystem.API.DTOs;
using HRSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.API.Services
{

    public class BranchService : IBranchService
    {
        private readonly ApplicationDbContext _context;

        public BranchService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🟢 Get All
        public async Task<List<BranchDto>> GetAllAsync(int tenantId)
        {
            return await _context.Branches
                .Where(b => b.TenantId == tenantId)
                .Select(b => new BranchDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address
                })
                .ToListAsync();
        }

        // 🟢 Get By Id (مع Tenant)
        public async Task<BranchDto?> GetByIdAsync(int id, int tenantId)
        {
            return await _context.Branches
                .Where(b => b.Id == id && b.TenantId == tenantId)
                .Select(b => new BranchDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address
                })
                .FirstOrDefaultAsync();
        }

        // 🟢 Create
        
        public async Task<BranchDto> CreateAsync(CreateBranchDto dto, int tenantId)
        {
            var branch = new Branch
            {
                Name = dto.Name,
                Address = dto.Address,

                TenantId = tenantId // مهم جدًا
            };

            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();

            return new BranchDto
            {
                Name = branch.Name,
                Address = branch.Address
            };
        }

        // 🟢 Update
        public async Task<bool> UpdateAsync(int id, int tenantId, UpdateBranchDto dto)
        {
            var branch = await _context.Branches
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);

            if (branch == null)
                return false;

            branch.Name = dto.Name;
            branch.Address = dto.Address;

            await _context.SaveChangesAsync();
            return true;
        }

        // 🔴 Delete
        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            var branch = await _context.Branches
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);

            if (branch == null)
                return false;

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}