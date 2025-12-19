using Microsoft.EntityFrameworkCore;
using Suppliers.API.Data;
using Suppliers.API.Models;

namespace Suppliers.API.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _context;

        public SupplierService(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL + SEARCH
        public async Task<List<Supplier>> GetAllAsync(string? search)
        {
            var query = _context.Suppliers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.SupplierName.Contains(search) ||
                    (s.Email != null && s.Email.Contains(search)));
            }

            return await query.ToListAsync();
        }

        // GET BY ID
        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        // CREATE (WITH BUSINESS RULES)
        public async Task<Supplier> CreateAsync(Supplier supplier)
        {
            // Business rule: duplicate name
            if (await _context.Suppliers.AnyAsync(s => s.SupplierName == supplier.SupplierName))
                throw new Exception("Supplier name already exists.");

            // Business rule: duplicate email
            if (!string.IsNullOrEmpty(supplier.Email) &&
                await _context.Suppliers.AnyAsync(s => s.Email == supplier.Email))
                throw new Exception("Supplier email already exists.");

            supplier.IsActive = true;
            supplier.CreatedAt = DateTime.UtcNow;

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return supplier;
        }

        // UPDATE
        public async Task<bool> UpdateAsync(int id, Supplier supplier)
        {
            var existing = await _context.Suppliers.FindAsync(id);
            if (existing == null) return false;

            // Business rule: inactive supplier cannot be updated
            if (!existing.IsActive)
                throw new Exception("Inactive suppliers cannot be updated.");

            existing.SupplierName = supplier.SupplierName;
            existing.Email = supplier.Email;
            existing.Phone = supplier.Phone;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return false;

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        // ACTIVATE / DEACTIVATE
        public async Task<bool> UpdateStatusAsync(int id, bool isActive)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return false;

            supplier.IsActive = isActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
