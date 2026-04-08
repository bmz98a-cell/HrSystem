using HRSystem.API.Data;
using HRSystem.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TenantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(Tenant tenant)
        {
            _context.Tenants.Add(tenant);
            _context.SaveChanges();
            return Ok(tenant);
        }
    }
}