namespace GoodHamburguer.Models.DTOs;

// ─── Request ────────────────────────────────────────────────────────────────

public record OrderRequest(List<string> ItemCodes);

// ─── Responses ──────────────────────────────────────────────────────────────

public record MenuItemResponse(
    string Code,
    string Name,
    decimal Price,
    string Category);

public record OrderItemResponse(
    string Code,
    string Name,
    decimal Price,
    string Category);

public record OrderResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<OrderItemResponse> Items,
    decimal Subtotal,
    decimal DiscountPercent,
    decimal DiscountAmount,
    decimal Total);

public record ErrorResponse(string Error, IEnumerable<string>? Details = null);
