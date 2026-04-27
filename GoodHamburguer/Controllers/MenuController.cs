using GoodHamburguer.Models.DTOs;
using GoodHamburguer.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Controllers;

[ApiController]
[Route("api/menu")]
[Produces("application/json")]
public class MenuController : ControllerBase
{
    /// <summary>Retorna todos os itens do cardápio.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MenuItemResponse>), StatusCodes.Status200OK)]
    public IActionResult GetMenu()
    {
        var response = Menu.Items.Select(i =>
            new MenuItemResponse(i.Code, i.Name, i.Price, i.Category.ToString()));

        return Ok(response);
    }
}
