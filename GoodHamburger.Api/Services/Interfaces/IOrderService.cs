using GoodHamburger.Models.DTOs;

namespace GoodHamburger.Api.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<OrderResponse?> GetByIdAsync(Guid id);
        Task<(OrderResponse? order, ErrorResponse? error)> CreateAsync(OrderRequest request);
        Task<(OrderResponse? order, ErrorResponse? error)> UpdateAsync(Guid id, OrderRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
