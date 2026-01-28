using Microsoft.AspNetCore.Mvc;
using RealEstate.Business.Abstract;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyImagesController : ControllerBase
{
    private readonly IPropertyImageService _propertyImageService;

    public PropertyImagesController(IPropertyImageService propertyImageService)
    {
        _propertyImageService = propertyImageService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _propertyImageService.GetAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? propertyId = null)
    {
        try
        {
            var result = await _propertyImageService.GetAllAsync(
                predicate: null,
                orderBy: null,
                propertyId: propertyId,
                isDeleted: false
            );
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet("property/{propertyId}")]
    public async Task<IActionResult> GetByPropertyId(int propertyId)
    {
        try
        {
            var result = await _propertyImageService.GetByPropertyIdAsync(propertyId);
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
            var result = await _propertyImageService.CountAsync();
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet("count/property/{propertyId}")]
    public async Task<IActionResult> GetCountByPropertyId(int propertyId)
    {
        try
        {
            var result = await _propertyImageService.CountByPropertyIdAsync(propertyId);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PropertyImageCreateDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _propertyImageService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PropertyImageUpdateDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _propertyImageService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPut("{id}/set-primary")]
    public async Task<IActionResult> SetAsPrimary(int id, [FromQuery] int propertyId)
    {
        try
        {
            var result = await _propertyImageService.SetAsPrimaryAsync(id, propertyId);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPut("{id}/display-order")]
    public async Task<IActionResult> UpdateDisplayOrder(int id, [FromBody] int displayOrder)
    {
        try
        {
            var result = await _propertyImageService.UpdateDisplayOrderAsync(id, displayOrder);
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
            var result = await _propertyImageService.SoftDeleteAsync(id);
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
            var result = await _propertyImageService.HardDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpDelete("property/{propertyId}/all")]
    public async Task<IActionResult> DeleteAllByPropertyId(int propertyId)
    {
        try
        {
            var result = await _propertyImageService.DeleteAllByPropertyIdAsync(propertyId);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }
}