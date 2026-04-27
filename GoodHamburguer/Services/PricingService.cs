using GoodHamburger.Models;
using GoodHamburguer.Models.Enums;
using GoodHamburguer.Models.Models;

namespace GoodHamburguer.Api.Services;

/// <summary>
/// Encapsula todas as regras de desconto do cardápio.
/// Regras (aplicadas na ordem — apenas uma pode ser ativada por pedido):
///   Sanduíche + batata + refrigerante → 20 %
///   Sanduíche + refrigerante          → 15 %
///   Sanduíche + batata                → 10 %
///   Apenas sanduíche (ou sem combo)   →  0 %
/// </summary>
public static class PricingService
{
    public static (decimal subtotal, decimal discountPercent, decimal discountAmount, decimal total)
        Calculate(IEnumerable<MenuItem> items)
    {
        var list = items.ToList();

        bool hasSandwich = list.Any(i => i.Category == ItemCategory.Sandwich);
        bool hasSide     = list.Any(i => i.Category == ItemCategory.Side);
        bool hasDrink    = list.Any(i => i.Category == ItemCategory.Drink);

        decimal subtotal = list.Sum(i => i.Price);

        decimal discountPercent = (hasSandwich, hasSide, hasDrink) switch
        {
            (true, true, true)   => 20m,
            (true, false, true)  => 15m,
            (true, true, false)  => 10m,
            _                    =>  0m,
        };

        decimal discountAmount = Math.Round(subtotal * discountPercent / 100m, 2);
        decimal total          = subtotal - discountAmount;

        return (subtotal, discountPercent, discountAmount, total);
    }
}
