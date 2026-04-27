using System.Net.Http.Json;
using GoodHamburger.Models.DTOs;

namespace GoodHamburger.App.Services;

public class OrderApiService
{
    private readonly HttpClient _http;

    public OrderApiService(HttpClient http) => _http = http;

    public Task<List<MenuItemDto>?> GetMenuAsync() =>
        _http.GetFromJsonAsync<List<MenuItemDto>>("api/menu");

    public Task<List<OrderDto>?> GetOrdersAsync() =>
        _http.GetFromJsonAsync<List<OrderDto>>("api/orders");

    public Task<OrderDto?> GetOrderAsync(Guid id) =>
        _http.GetFromJsonAsync<OrderDto>($"api/orders/{id}");

    public async Task<(OrderDto? order, string? error)> CreateOrderAsync(List<string> itemCodes)
    {
        var response = await _http.PostAsJsonAsync("api/orders", new CreateOrderDto(itemCodes));
        if (response.IsSuccessStatusCode)
            return (await response.Content.ReadFromJsonAsync<OrderDto>(), null);

        var err = await response.Content.ReadAsStringAsync();
        return (null, err);
    }

    public async Task<(OrderDto? order, string? error)> UpdateOrderAsync(Guid id, List<string> itemCodes)
    {
        var response = await _http.PutAsJsonAsync($"api/orders/{id}", new CreateOrderDto(itemCodes));
        if (response.IsSuccessStatusCode)
            return (await response.Content.ReadFromJsonAsync<OrderDto>(), null);

        var err = await response.Content.ReadAsStringAsync();
        return (null, err);
    }

    public async Task<bool> DeleteOrderAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/orders/{id}");
        return response.IsSuccessStatusCode;
    }
}
