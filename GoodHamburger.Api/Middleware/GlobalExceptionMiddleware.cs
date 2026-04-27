using GoodHamburger.Models.DTOs;
using System.Net;
using System.Text.Json;

namespace GoodHamburger.Api.Middleware;

/// <summary>
/// Captura exceções não tratadas e retorna JSON padronizado,
/// evitando que stack traces vazem para o cliente em produção.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado");

            context.Response.StatusCode  = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var body = JsonSerializer.Serialize(
                new ErrorResponse("Ocorreu um erro interno. Tente novamente mais tarde."),
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            await context.Response.WriteAsync(body);
        }
    }
}
