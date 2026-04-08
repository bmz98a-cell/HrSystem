namespace HRSystem.API.DTOs;

using System.ComponentModel.DataAnnotations;


public class CreateEmployeeDto
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }=  string.Empty;

    [Required(ErrorMessage = "Phone is required")]
    public string Phone { get; set; }=  string.Empty;

    [Range(1, 100000, ErrorMessage = "Salary must be between 1 and 9,999,999")]
    public decimal Salary { get; set; }

    [Required(ErrorMessage = "Branch is required")]
    public int BranchId { get; set; }
}