using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RealEstate.Business.DTOs.ResponseDto;

namespace RealEstate.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
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
            _logger.LogError(ex, "Beklenmeyen bir hata oluştu: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = exception switch
        {
            DbUpdateConcurrencyException concurrencyEx => new
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                Response = ResponseDto<object>.Fail("Eşzamanlılık hatası: Kayıt başka bir kullanıcı tarafından değiştirilmiş", 409)
            },
            DbUpdateException dbEx => new
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                Response = ResponseDto<object>.Fail($"Veritabanı güncelleme hatası: {dbEx.InnerException?.Message ?? dbEx.Message}", 409)
            },
            ArgumentNullException argNullEx => new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Response = ResponseDto<object>.Fail($"Gerekli parametre eksik: {argNullEx.ParamName}", 400)
            },
            ArgumentException argEx => new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Response = ResponseDto<object>.Fail($"Geçersiz parametre: {argEx.Message}", 400)
            },
            InvalidOperationException invalidOpEx => new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Response = ResponseDto<object>.Fail($"Geçersiz işlem: {invalidOpEx.Message}", 400)
            },
            TimeoutException timeoutEx => new
            {
                StatusCode = (int)HttpStatusCode.GatewayTimeout,
                Response = ResponseDto<object>.Fail("İşlem zaman aşımına uğradı", 504)
            },
            UnauthorizedAccessException => new
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Response = ResponseDto<object>.Fail("Yetkisiz erişim", 401)
            },
            KeyNotFoundException => new
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Response = ResponseDto<object>.Fail("Kayıt bulunamadı", 404)
            },
            _ => new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Response = ResponseDto<object>.Fail("Sunucu hatası oluştu", 500)
            }
        };

        context.Response.StatusCode = response.StatusCode;

        var jsonResponse = JsonSerializer.Serialize(response.Response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}