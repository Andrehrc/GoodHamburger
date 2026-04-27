using GoodHamburguer.Models.Enums;

namespace GoodHamburguer.Models.Models;

public class MenuItem
{
    public int Id { get; set; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public ItemCategory Category { get; init; }
}

public static class Menu
{
    public static readonly IReadOnlyList<MenuItem> Items = new List<MenuItem>
    {
        new() {Id = 1, Code = "X-BURGER", Name = "X Burger",    Price = 5.00m, Category = ItemCategory.Sandwich },
        new() {Id = 2, Code = "X-EGG",   Name = "X Egg",       Price = 4.50m, Category = ItemCategory.Sandwich },
        new() {Id = 3, Code = "X-BACON", Name = "X Bacon",     Price = 7.00m, Category = ItemCategory.Sandwich },
        new() {Id = 4, Code = "FRIES",   Name = "Batata Frita", Price = 2.00m, Category = ItemCategory.Side     },
        new() {Id = 5, Code = "SODA",    Name = "Refrigerante", Price = 2.50m, Category = ItemCategory.Drink    },
    };

    public static MenuItem? FindByCode(string code) =>
        Items.FirstOrDefault(i => i.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
}
