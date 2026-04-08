namespace HRSystem.API.Models
{
    public class License
    {
        public int Id { get; set; }
        public int TenantId { get; set; }

        public int MaxUsers { get; set; }
        public int MaxEmployees { get; set; }
        public int MaxBranches { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
   
}
