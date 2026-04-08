using HRSystem.API.Data;
using HRSystem.API.DTOs;
using HRSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginRequest = HRSystem.API.DTOs.LoginRequest;

namespace HRSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // =========================
        // 🔐 REGISTER
        // =========================
        [Authorize]
        [HttpPost("register")]
        public IActionResult Register(RegisterUserDto dto)
        {
            var tenantId = int.Parse(User.FindFirst("tenantId")!.Value);

            var exists = _context.Users.Any(u =>
                u.Username == dto.Username &&
                u.TenantId == tenantId
            );

            if (exists)
            {
                return BadRequest("User already exists in this tenant");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

         
        

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = passwordHash,
                TenantId = tenantId // ✅ من التوكن
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User created successfully");
        }

        // =========================
        // 🔐 LOGIN
        // =========================
        [HttpPost("login")]
        public IActionResult Login(LoginRequest dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == dto.Username);

            if (user == null)
                return Unauthorized();

            var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isValid)
                return Unauthorized();

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim("tenantId", user.TenantId.ToString())
    };

            var keyValue = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(keyValue))
                throw new Exception("JWT Key is missing");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue)); 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString }); // 🔥 هذا المهم
        }

        // =========================
        // 🔥 GET PERMISSIONS
        // =========================
        private async Task<List<string>> GetUserPermissions(int userId)
        {
            var direct = await _context.UserPermissions
                .Where(x => x.UserId == userId)
                .Include(x => x.Permission)
                .Select(x => x.Permission.Name)
                .ToListAsync();

            var fromRoles = await _context.UserRoles
                .Where(x => x.UserId == userId)
                .Include(x => x.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .SelectMany(x => x.Role.RolePermissions)
                .Select(x => x.Permission.Name)
                .ToListAsync();

            return direct.Union(fromRoles).ToList();
        }

        // =========================
        // 🔑 GENERATE JWT
        // =========================
        private async Task<string> GenerateJwtToken(User user)
        {
            var permissions = await GetUserPermissions(user.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("TenantId", user.TenantId.ToString())
            };

            if (permissions != null)
            {
                foreach (var p in permissions)
                {
                    if (!string.IsNullOrWhiteSpace(p))
                    {
                        claims.Add(new Claim("permission", p));
                    }
                }
            }

            var keyString = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(keyString))
                throw new Exception("JWT KEY IS NULL 💣");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(keyString)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}