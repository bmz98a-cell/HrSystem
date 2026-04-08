using Microsoft.EntityFrameworkCore;
using HRSystem.API.Data;
using HRSystem.API.Models;


public interface ILicenseService
{
    Task CheckLicenseExpiry();
    Task CheckUserLimit();
    Task CheckEmployeeLimit();
    Task CheckBranchLimit(); // 🔥 مهم جدا تضيفها
}

public class LicenseService : ILicenseService
{
    private readonly ApplicationDbContext _context;

    public LicenseService(ApplicationDbContext context)
    {
        _context = context;
    }

    // 🟢 ميثود عامة تجيب اللايسنس
    public async Task<License> GetLicense()
    {
        var license = await _context.Licenses.FirstOrDefaultAsync();

        if (license == null)
            throw new Exception("License not configured");

        return license;
    }

    // 🟢 التحقق من انتهاء الاشتراك
    public async Task CheckLicenseExpiry()
    {
        var license = await GetLicense();

        if (license.ExpiryDate < DateTime.Now)
            throw new Exception("انتهى الاشتراك");
    }

    // 🟢 الحد الأقصى للمستخدمين
    public async Task CheckUserLimit()
    {
        var license = await GetLicense();

        var userCount = await _context.Users.CountAsync();

        if (userCount >= license.MaxUsers)
            throw new Exception("تم الوصول إلى الحد الأقصى للمستخدمين");
    }

    // 🟢 الحد الأقصى للموظفين
    public async Task CheckEmployeeLimit()
    {
        var license = await GetLicense();

        var empCount = await _context.Employees.CountAsync();

        if (empCount >= license.MaxEmployees)
            throw new Exception("تم الوصول إلى الحد الأقصى للموظفين");
    }

    // 🟢 الحد الأقصى للفروع
    public async Task CheckBranchLimit()
    {
        var license = await GetLicense();

        var branchCount = await _context.Branches.CountAsync();

        if (branchCount >= license.MaxBranches)
            throw new Exception("تم الوصول إلى الحد الأقصى للفروع");
    }
}