using HRSystem.API.DTOs;

public interface IEmployeeService
{
    Task<List<EmployeeResponseDto>> GetAllAsync(int? branchId, int page, int pageSize, string? search);

    Task<EmployeeResponseDto?> GetByIdAsync(int id, int tenantId);

    Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto, int tenantId);

    Task<bool> UpdateAsync(int id, int tenantId, UpdateEmployeeDto dto);

    Task<bool> DeleteAsync(int id, int tenantId);
}