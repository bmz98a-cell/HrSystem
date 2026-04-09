using HRSystem.API.DTOs;

public interface IEmployeeService
{
    Task<List<EmployeeResponseDto>> GetAllAsync(int branchId, int page, int pageSize, string? search);

    Task<EmployeeResponseDto?> GetByIdAsync(int id);

    Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto);

    Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto);

    Task<bool> DeleteAsync(int id);
}