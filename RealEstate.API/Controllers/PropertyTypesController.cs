using Microsoft.AspNetCore.Mvc;
using RealEstate.Business.Abstract;
using RealEstate.Business.DTOs.PropertyTypeDto;
using RealEstate.Business.DTOs.ResponseDto;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyTypesController : ControllerBase
{
    private readonly IPropertyTypeService _propertyTypeService;

    public PropertyTypesController(IPropertyTypeService propertyTypeService)
    {
        _propertyTypeService = propertyTypeService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _propertyTypeService.GetAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _propertyTypeService.GetAllAsync();
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
            var result = await _propertyTypeService.CountAsync();
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PropertyTypeCreateDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _propertyTypeService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PropertyTypeUpdateDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _propertyTypeService.UpdateAsync(id, updateDto);
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
            var result = await _propertyTypeService.SoftDeleteAsync(id);
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
            var result = await _propertyTypeService.HardDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }
}