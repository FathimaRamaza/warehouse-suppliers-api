using Suppliers.API.Models;

namespace Suppliers.API.Services
{
    public interface ISupplierService
    {
        Task<List<Supplier>> GetAllAsync(
            int page,
            int pageSize,
            string? search,
            string? sortBy,
            string? sortOrder,
            bool? isActive);

        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier> CreateAsync(Supplier supplier);
        Task<bool> UpdateAsync(int id, Supplier supplier);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStatusAsync(int id, bool isActive);
    }
}