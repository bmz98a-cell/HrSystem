using HRSystem.API.Data;
using HRSystem.API.DTOs;
using HRSystem.API.Services;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchesController : BaseController
    {
        private readonly IBranchService _service;

        public BranchesController(IBranchService service)
        {
            _service = service;
        }

        // ✅ GET ALL
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int tenantId = TenantId;

            var result = await _service.GetAllAsync(tenantId);

            return Ok(result);
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            int tenantId = TenantId;

            var result = await _service.GetByIdAsync(id, tenantId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // ✅ POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBranchDto dto)
        {
            int tenantId = TenantId;

            var result = await _service.CreateAsync(dto, tenantId);

            return Ok(result);
        }

        // ✅ PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchDto dto)
        {
            int tenantId = TenantId;

            var result = await _service.UpdateAsync(id, tenantId, dto);

            if (!result)
                return NotFound();

            return Ok();
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int tenantId = TenantId;

            var result = await _service.DeleteAsync(id, tenantId);

            if (!result)
                return NotFound();

            return Ok();
        }
    }
}