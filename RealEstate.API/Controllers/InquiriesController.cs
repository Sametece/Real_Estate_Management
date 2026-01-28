using Microsoft.AspNetCore.Mvc;
using RealEstate.Business.Abstract;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.Inquiry;
using RealEstate.Business.DTOs.ResponseDto;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InquiriesController : ControllerBase
{
    private readonly IInguiryService _inquiryService;

    public InquiriesController(IInguiryService inquiryService)
    {
        _inquiryService = inquiryService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _inquiryService.GetAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? propertyId = null, [FromQuery] bool includeProperty = false)
    {
        try
        {
            var result = await _inquiryService.GetAllAsync(
                predicate: null,
                orderBy: null,
                includeEProperty: includeProperty,
                epropertyId: propertyId,
                isDeleted: false
            );
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetAllPaged([FromQuery] EPropertyPaginationQueryDto paginationQuery, [FromQuery] int? propertyId = null, [FromQuery] bool includeProperty = false)
    {
        try
        {
            var result = await _inquiryService.GetAllPagedAsync(
                paginationQuery,
                predicate: null,
                orderBy: null,
                includeEProperty: includeProperty,
                epropertyId: propertyId,
                isDeleted: false
            );
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        try
        {
            var result = await _inquiryService.CountAsync();
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InquiryCreateDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _inquiryService.InquiryCreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateStatus([FromBody] InquiryAdminUpdateDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _inquiryService.UpdateAsync(updateDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        try
        {
            var result = await _inquiryService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpDelete("hard/{id}")]
    public async Task<IActionResult> HardDelete(int id)
    {
        try
        {
            var result = await _inquiryService.HardDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }
}