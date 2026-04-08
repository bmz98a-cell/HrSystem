using HRSystem.API.Data;
using HRSystem.API.DTOs;
using HRSystem.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.API.Controllers

{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]


    public class EmployeesController : BaseController
    {
       


        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [HttpGet]
        public async Task<IActionResult> Get(
    [FromQuery] int? branchId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null)
        {
            var result = await _employeeService.GetAllAsync(branchId, page, pageSize, search);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tenantId = TenantId;
            var result = await _employeeService.GetByIdAsync(id, tenantId);

            return result == null ? NotFound() : Ok(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        //{
        //    var tenantId = TenantId;

        //    var result = await _employeeService.CreateAsync(dto, tenantId);

        //    return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        //}
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            var result = await _employeeService.CreateAsync(dto, TenantId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
        {
            var tenantId = TenantId;

            var updated = await _employeeService.UpdateAsync(id, tenantId, dto);

            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tenantId = TenantId;

            var deleted = await _employeeService.DeleteAsync(id, tenantId);

            return deleted ? NoContent() : NotFound();
        }
    }
}