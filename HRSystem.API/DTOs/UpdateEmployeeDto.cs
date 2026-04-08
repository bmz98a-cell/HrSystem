namespace HRSystem.API.DTOs
{
    public class UpdateEmployeeDto
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public decimal Salary { get; set; }

        public int BranchId { get; set; }
    }
}