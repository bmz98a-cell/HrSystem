namespace HRSystem.API.DTOs
{
    public class BranchResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
    }
}
