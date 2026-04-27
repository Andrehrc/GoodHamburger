using GoodHamburger.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using GoodHamburger.Models.Models;
using GoodHamburger.Api.Context;

namespace GoodHamburger.Api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Order>> GetAllAsync()
            => await _db.Orders.OrderBy(o => o.CreatedAt).ToListAsync();

        public async Task<Order?> GetByIdAsync(Guid id)
            => await _db.Orders.FindAsync(id);

        public async Task AddAsync(Order order)
            => await _db.Orders.AddAsync(order);

        public Task UpdateAsync(Order order)
        {
            _db.Orders.Update(order);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Order order)
        {
            _db.Orders.Remove(order);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
            => await _db.SaveChangesAsync();
    }
}
