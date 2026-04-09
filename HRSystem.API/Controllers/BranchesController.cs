using HRSystem.API.DTOs;
using HRSystem.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchService _service;
        private readonly ITenantProvider _tenantProvider;

        public BranchesController(IBranchService service, ITenantProvider tenantProvider)
        {
            _service = service;
            _tenantProvider = tenantProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tenantId = _tenantProvider.GetTenantId();
            var result = await _service.GetAllAsync(tenantId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tenantId = _tenantProvider.GetTenantId();
            var result = await _service.GetByIdAsync(id, tenantId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBranchDto dto)
        {
            var tenantId = _tenantProvider.GetTenantId();
            var result = await _service.CreateAsync(dto, tenantId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchDto dto)
        {
            var tenantId = _tenantProvider.GetTenantId();
            var success = await _service.UpdateAsync(id, tenantId, dto);

            if (!success)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tenantId = _tenantProvider.GetTenantId();
            var success = await _service.DeleteAsync(id, tenantId);

            if (!success)
                return NotFound();

            return Ok();
        }
    }
}