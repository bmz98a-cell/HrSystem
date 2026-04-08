using HRSystem.API.Data;
using HRSystem.API.DTOs;
using HRSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.API.Services
{

   
    public class EmployeeService : IEmployeeService
    {
                     private readonly ITenantProvider _tenantProvider;
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context, ITenantProvider tenantProvider)
        
            {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public async Task<List<EmployeeResponseDto>> GetAllAsync(
    int? branchId,
    int page,
    int pageSize,
    string? search)

        {

            var tenantIdFromToken = _tenantProvider.GetTenantId();
            var tenantId = _tenantProvider.GetTenantId();

            var query = _context.Employees
                .Include(e => e.Branch)
                .Where(e => e.TenantId == tenantId);

            if (branchId.HasValue)
                query = query.Where(e => e.BranchId == branchId.Value);

            // 🔥 إضافة البحث
            if (!string.IsNullOrEmpty(search))
                query = query.Where(e => e.Name.ToLower().Contains(search.ToLower()));

            var result = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    
                    Name = e.Name,
                    Email = e.Email,
                    Phone = e.Phone,
                    Salary = e.Salary,
                    BranchId = e.BranchId,
                    BranchName = e.Branch.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(int id, int tenantId)
        {
            return await _context.Employees
                .Include(e => e.Branch)
                .Where(e => e.Id == id && e.TenantId == tenantId)
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    
                    Name = e.Name,
                    Email = e.Email,
                    Phone = e.Phone,
                    Salary = e.Salary,
                    BranchId = e.BranchId,
                    BranchName = e.Branch != null ? e.Branch.Name : ""
                })
                .FirstOrDefaultAsync();
        }

        public async Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto, int tenantId)
        {
            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.Id == dto.BranchId && b.TenantId == tenantId);

            if (branch == null)
                throw new Exception("Branch not found or not belongs to this tenant");

            var employee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Salary = dto.Salary,
                BranchId = dto.BranchId,
                TenantId = tenantId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return new EmployeeResponseDto
            {
                Id = employee.Id,
                
                Name = employee.Name,
                Email = employee.Email,
                Phone = employee.Phone,
                Salary = employee.Salary,
                BranchId = employee.BranchId,
                BranchName = branch.Name // 🔥 الحل هنا
            };
        }

        public async Task<bool> UpdateAsync(int id, int tenantId, UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.TenantId == tenantId);

            if (employee == null)
                return false;

            var branch = await _context.Branches
                .FirstOrDefaultAsync(b => b.Id == dto.BranchId && b.TenantId == tenantId);

            if (branch == null)
                throw new Exception("Invalid Branch");

            employee.Name = dto.Name;
            employee.Email = dto.Email;
            employee.Phone = dto.Phone;
            employee.Salary = dto.Salary;
            employee.BranchId = dto.BranchId;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.TenantId == tenantId);

            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}