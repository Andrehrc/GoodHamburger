using GoodHamburger.Models.Models;

namespace GoodHamburger.Api.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
        Task SaveChangesAsync();
    }
}
