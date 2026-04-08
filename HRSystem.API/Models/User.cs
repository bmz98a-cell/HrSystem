namespace HRSystem.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = default!;

        public string PasswordHash { get; set; } = string.Empty;

        public int TenantId { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
       
          

           
        
    }
}