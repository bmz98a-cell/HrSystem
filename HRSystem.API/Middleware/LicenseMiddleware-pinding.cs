using HRSystem.API.Services;

namespace HRSystem.API.Middleware
{
    public class LicenseMiddleware
    {
        private readonly RequestDelegate _next;

        public LicenseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILicenseService licenseService)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            if (context.Request.Method != "GET")
            {
                try
                {
                    await licenseService.CheckLicenseExpiry();

                    // موظفين
                    if (path.Contains("employee"))
                        await licenseService.CheckEmployeeLimit();

                    // مستخدمين (register)
                    if (path.Contains("register"))
                        await licenseService.CheckUserLimit();

                    // فروع
                    if (path.Contains("branch"))
                        await licenseService.CheckBranchLimit();
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync(ex.Message);
                    return;
                }
            }

            await _next(context);
        }
    }
}