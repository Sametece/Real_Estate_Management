using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Business.Abstract;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz kullanıcı", 400));
            }

            var result = await _userService.GetProfileAsync(userId);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UserUpdateDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz kullanıcı", 400));
            }

            var result = await _userService.UpdateProfileAsync(userId, updateDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPut("me/agent-info")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> UpdateAgentInfo([FromBody] AgentUpdateDto agentUpdateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz kullanıcı", 400));
            }

            var result = await _userService.UpdateAgentInfoAsync(userId, agentUpdateDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _userService.GetAllUsersAsync(
                predicate: null,
                orderBy: null,
                pageNumber: pageNumber,
                pageSize: pageSize
            );
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var result = await _userService.GetUserByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPut("{id}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserRole(int id, [FromBody] string roleName)
    {
        try
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest(ResponseDto<object>.Fail("Rol adı gereklidir", 400));
            }

            var result = await _userService.UpdateUserRoleAsync(id, roleName);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet("count")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserCount()
    {
        try
        {
            var result = await _userService.GetUserCountAsync();
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpGet("agents/count")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAgentCount()
    {
        try
        {
            var result = await _userService.GetAgentCountAsync();
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var result = await _userService.SoftDeleteUserAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }
}