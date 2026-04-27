using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodHamburger.Models.Models;

public class Order
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public decimal Subtotal { get; set; }

    public decimal DiscountPercent { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal Total { get; set; }

    public List<string> ItemCodes { get; set; } = new();
}
