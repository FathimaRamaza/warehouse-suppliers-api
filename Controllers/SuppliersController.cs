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

        // GET: api/suppliers
        [HttpGet]
        public async Task<IActionResult> GetSuppliers(
            int page = 1,
            int pageSize = 10,
            string? search = null,
            string? sortBy = null,
            string? sortOrder = "asc",
            bool? isActive = null)
        {
            var suppliers = await _service.GetAllAsync(
                page,
                pageSize,
                search,
                sortBy,
                sortOrder,
                isActive);

            return Ok(suppliers);
        }

        // GET: api/suppliers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var supplier = await _service.GetByIdAsync(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        // POST: api/suppliers
        [HttpPost]
        public async Task<IActionResult> CreateSupplier(Supplier supplier)
        {
            var created = await _service.CreateAsync(supplier);
            return Ok(created);
        }

        // PUT: api/suppliers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, Supplier supplier)
        {
            var updated = await _service.UpdateAsync(id, supplier);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE: api/suppliers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // PATCH: api/suppliers/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, bool isActive)
        {
            var updated = await _service.UpdateStatusAsync(id, isActive);
            if (!updated) return NotFound();
            return NoContent();
        }
    }
}