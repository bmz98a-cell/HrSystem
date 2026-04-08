namespace HRSystem.API.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ConnectionString { get; set; }
    }
}