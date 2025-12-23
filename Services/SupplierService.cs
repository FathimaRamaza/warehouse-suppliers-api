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

        // GET ALL + SEARCH + FILTER + SORT + PAGINATION
        public async Task<List<Supplier>> GetAllAsync(
            int page,
            int pageSize,
            string? search,
            string? sortBy,
            string? sortOrder,
            bool? isActive)
        {
            var query = _context.Suppliers.AsQueryable();

            //  FILTERING
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.SupplierName.Contains(search) ||
                    (s.Email != null && s.Email.Contains(search)));
            }

            if (isActive.HasValue)
            {
                query = query.Where(s => s.IsActive == isActive.Value);
            }

            //  SORTING
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.ToLower() == "name")
                {
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(s => s.SupplierName)
                        : query.OrderBy(s => s.SupplierName);
                }
                else if (sortBy.ToLower() == "createdat")
                {
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(s => s.CreatedAt)
                        : query.OrderBy(s => s.CreatedAt);
                }
            }

            //  PAGINATION
            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);

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
            if (await _context.Suppliers.AnyAsync(s => s.SupplierName == supplier.SupplierName))
                throw new Exception("Supplier name already exists.");

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