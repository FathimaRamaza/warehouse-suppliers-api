using Suppliers.API.Models;

namespace Suppliers.API.Services
{
    public interface ISupplierService
    {
        Task<List<Supplier>> GetAllAsync(string? search);
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier> CreateAsync(Supplier supplier);
        Task<bool> UpdateAsync(int id, Supplier supplier);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStatusAsync(int id, bool isActive);
    }
}
