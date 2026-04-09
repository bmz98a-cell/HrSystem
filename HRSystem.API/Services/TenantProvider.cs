using HRSystem.API.Services;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetTenantId()
    {
        var tenantId = _httpContextAccessor.HttpContext?
            .User.FindFirst("tenantId")?.Value;

        if (string.IsNullOrEmpty(tenantId))
            throw new Exception("Tenant not found in token");

        return int.Parse(tenantId);
    }
}