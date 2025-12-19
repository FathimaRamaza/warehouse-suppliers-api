using Microsoft.AspNetCore.Mvc;
using Suppliers.API.Models;
using Suppliers.API.Services;

namespace Suppliers.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _service;

        public SuppliersController(ISupplierService service)
        {
            _service = service;
        }

        // GET /api/suppliers?search=abc
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            return Ok(await _service.GetAllAsync(search));
        }

        // GET /api/suppliers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _service.GetByIdAsync(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        // POST /api/suppliers
        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            try
            {
                var created = await _service.CreateAsync(supplier);
                return CreatedAtAction(nameof(GetById),
                    new { id = created.SupplierId }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/suppliers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Supplier supplier)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, supplier);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/suppliers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // PUT /api/suppliers/{id}/status?isActive=true
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] bool isActive)
        {
            var updated = await _service.UpdateStatusAsync(id, isActive);
            if (!updated) return NotFound();
            return NoContent();
        }
    }
}
