using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Suppliers.API.Data;
using Suppliers.API.Models;

namespace Suppliers.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/suppliers?search=abc
        [HttpGet]
        public async Task<IActionResult> GetSuppliers([FromQuery] string? search)
        {
            IQueryable<Supplier> query = _context.Suppliers;

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.SupplierName.Contains(search) ||
                    (s.Email != null && s.Email.Contains(search)) ||
                    (s.Phone != null && s.Phone.Contains(search))
                );
            }

            var suppliers = await query.ToListAsync();
            return Ok(suppliers);
        }

        // GET: api/suppliers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        // POST: api/suppliers
        [HttpPost]
        public async Task<IActionResult> CreateSupplier(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetSupplierById),
                new { id = supplier.SupplierId },
                supplier
            );
        }

        // PUT: api/suppliers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, Supplier updatedSupplier)
        {
            var existingSupplier = await _context.Suppliers.FindAsync(id);

            if (existingSupplier == null)
                return NotFound();

            existingSupplier.SupplierName = updatedSupplier.SupplierName;
            existingSupplier.Email = updatedSupplier.Email;
            existingSupplier.Phone = updatedSupplier.Phone;
            existingSupplier.IsActive = updatedSupplier.IsActive;

            await _context.SaveChangesAsync();

            return Ok(existingSupplier);
        }

        // DELETE: api/suppliers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound();

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}