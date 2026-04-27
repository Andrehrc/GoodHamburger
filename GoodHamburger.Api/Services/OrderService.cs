using GoodHamburger.Api.Repositories.Interfaces;
using GoodHamburger.Api.Services.Interfaces;
using GoodHamburger.Models.DTOs;
using GoodHamburger.Models.Enums;
using GoodHamburger.Models.Models;

namespace GoodHamburger.Api.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrderResponse>> GetAllAsync() =>
        (await _repository.GetAllAsync())
            .Select(MapToResponse);

    public async Task<OrderResponse?> GetByIdAsync(Guid id)
    {
        var order = await _repository.GetByIdAsync(id);
        return order is null ? null : MapToResponse(order);
    }

    public async Task<(OrderResponse? order, ErrorResponse? error)> CreateAsync(OrderRequest request)
    {
        var (resolvedItems, error) = Validate(request.ItemCodes);
        if (error is not null) return (null, error);

        var (subtotal, pct, discount, total) = PricingService.Calculate(resolvedItems!);

        var order = new Order
        {
            ItemCodes = request.ItemCodes.Select(c => c.ToUpperInvariant()).ToList(),
            Subtotal = subtotal,
            DiscountPercent = pct,
            DiscountAmount = discount,
            Total = total,
        };

        await _repository.AddAsync(order);
        await _repository.SaveChangesAsync();

        return (MapToResponse(order), null);
    }

    public async Task<(OrderResponse? order, ErrorResponse? error)> UpdateAsync(Guid id, OrderRequest request)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return (null, null);

        var (resolvedItems, error) = Validate(request.ItemCodes);
        if (error is not null) return (null, error);

        var (subtotal, pct, discount, total) = PricingService.Calculate(resolvedItems!);

        existing.ItemCodes = request.ItemCodes.Select(c => c.ToUpperInvariant()).ToList();
        existing.Subtotal = subtotal;
        existing.DiscountPercent = pct;
        existing.DiscountAmount = discount;
        existing.Total = total;
        existing.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(existing);
        await _repository.SaveChangesAsync();

        return (MapToResponse(existing), null);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var order = await _repository.GetByIdAsync(id);
        if (order is null) return false;

        await _repository.DeleteAsync(order);
        await _repository.SaveChangesAsync();
        return true;
    }

    private static (List<MenuItem>? items, ErrorResponse? error) Validate(List<string> codes)
    {
        if (codes is null || codes.Count == 0)
            return (null, new ErrorResponse("O pedido deve conter pelo menos um item."));

        var duplicates = codes
            .GroupBy(c => c.ToUpperInvariant())
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicates.Any())
            return (null, new ErrorResponse(
                "Itens duplicados não são permitidos.",
                duplicates.Select(d => $"Item duplicado: {d}")));

        var errors = new List<string>();
        var resolved = new List<MenuItem>();

        foreach (var code in codes)
        {
            var item = Menu.FindByCode(code);
            if (item is null) errors.Add($"Item não encontrado no cardápio: '{code}'.");
            else resolved.Add(item);
        }

        if (errors.Any())
            return (null, new ErrorResponse("Um ou mais itens são inválidos.", errors));

        var categoryErrors = new List<string>();
        if (resolved.Count(i => i.Category == ItemCategory.Sandwich) > 1)
            categoryErrors.Add("Apenas um sanduíche é permitido por pedido.");
        if (resolved.Count(i => i.Category == ItemCategory.Side) > 1)
            categoryErrors.Add("Apenas uma batata é permitida por pedido.");
        if (resolved.Count(i => i.Category == ItemCategory.Drink) > 1)
            categoryErrors.Add("Apenas um refrigerante é permitido por pedido.");

        if (categoryErrors.Any())
            return (null, new ErrorResponse("Limite de itens por categoria excedido.", categoryErrors));

        return (resolved, null);
    }

    private static OrderResponse MapToResponse(Order order)
    {
        var items = order.ItemCodes
            .Select(code => Menu.FindByCode(code)!)
            .Select(i => new OrderItemResponse(i.Code, i.Name, i.Price, i.Category.ToString()))
            .ToList();

        return new OrderResponse(
            order.Id, order.CreatedAt, order.UpdatedAt,
            items, order.Subtotal, order.DiscountPercent, order.DiscountAmount, order.Total);
    }
}
