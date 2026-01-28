using Microsoft.AspNetCore.Mvc;
using RealEstate.Business.Abstract;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EPropertiesController : ControllerBase
{
    private readonly IEPropertyService _ePropertyService;

    public EPropertiesController(IEPropertyService ePropertyService)
    {
        _ePropertyService = ePropertyService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _ePropertyService.GetAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? propertyTypeId = null, [FromQuery] bool includePropertyType = false)
    {
        try
        {
            var result = await _ePropertyService.GetAllAsync(
                predicate: null,
                orderBy: null,
                propertyType: includePropertyType,
                propertyTypeId: propertyTypeId,
                isdeleted: false
            );
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetAllPaged([FromQuery] EPropertyPaginationQueryDto paginationQuery, [FromQuery] int? propertyTypeId = null, [FromQuery] bool includePropertyType = false)
    {
        try
        {
            var result = await _ePropertyService.GetAllPagedAsync(
                paginationQuery,
                predicate: null,
                orderBy: null,
                propertyType: includePropertyType,
                propertyTypeId: propertyTypeId,
                isdeleted: false
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
            var result = await _ePropertyService.CountAsync();
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EPropertyCreateDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _ePropertyService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPut("admin/{id}")]
    public async Task<IActionResult> UpdateByAdmin(int id, [FromBody] EPropertyAdminUpdateDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _ePropertyService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPut("agent/{id}")]
    public async Task<IActionResult> UpdateByAgent(int id, [FromBody] EPropertyAgentUpdateDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _ePropertyService.Update2Async(id, updateDto);
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
            var result = await _ePropertyService.SoftDeleteAsync(id);
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
            var result = await _ePropertyService.HardDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }
}