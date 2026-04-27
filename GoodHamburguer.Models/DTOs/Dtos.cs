namespace GoodHamburguer.Models.DTOs;

public record MenuItemDto(string Code, string Name, decimal Price, string Category);

public record OrderItemDto(string Code, string Name, decimal Price, string Category);

public record OrderDto(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<OrderItemDto> Items,
    decimal Subtotal,
    decimal DiscountPercent,
    decimal DiscountAmount,
    decimal Total);

public record CreateOrderDto(List<string> ItemCodes);
