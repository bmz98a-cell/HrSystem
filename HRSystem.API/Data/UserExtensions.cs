using System.Security.Claims;
// test note --------
namespace HRSystem.API.Data
{
    public static class UserExtensions
    {
        public static int GetTenantId(this ClaimsPrincipal user)
        {
            var claim =
                user.FindFirst("tenantId") ??
                user.FindFirst("TenantId") ??
                user.FindFirst("tenant_id");

            if (claim == null)
                return 0; // 👈 بدل ما نرمي Exception

            return int.Parse(claim.Value);
        }
    }
    public class ApiResponse
    {
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}