using GoodHamburger.Api.Services.Interfaces;
using GoodHamburger.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Controllers;

[ApiController]
[Route("api/orders")]
[Produces("application/json")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    /// <summary>Lista todos os pedidos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() => Ok(await _orderService.GetAllAsync());

    /// <summary>Busca um pedido pelo ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _orderService.GetByIdAsync(id);
        return order is null
            ? NotFound(new ErrorResponse($"Pedido '{id}' não encontrado."))
            : Ok(order);
    }

    /// <summary>Cria um novo pedido.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] OrderRequest request)
    {
        var (order, error) = await _orderService.CreateAsync(request);
        if (error is not null) return BadRequest(error);
        return CreatedAtAction(nameof(GetById), new { id = order!.Id }, order);
    }

    /// <summary>Substitui os itens de um pedido existente.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] OrderRequest request)
    {
        var (order, error) = await _orderService.UpdateAsync(id, request);
        if (order is null && error is null) return NotFound(new ErrorResponse($"Pedido '{id}' não encontrado."));
        if (error is not null) return BadRequest(error);
        return Ok(order);
    }

    /// <summary>Remove um pedido.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await _orderService.DeleteAsync(id))
            return NotFound(new ErrorResponse($"Pedido '{id}' não encontrado."));
        return NoContent();
    }
}
