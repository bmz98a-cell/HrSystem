using HRSystem.API.DTOs;

namespace HRSystem.API.Services
{
    public interface IBranchService
    {
        Task<List<BranchDto>> GetAllAsync(int tenantId);

        Task<BranchDto?> GetByIdAsync(int id, int tenantId);

        Task<BranchDto> CreateAsync(CreateBranchDto dto, int tenantId);

        Task<bool> UpdateAsync(int id, int tenantId, UpdateBranchDto dto);
        Task<bool> DeleteAsync(int id, int tenantId);

    }
}