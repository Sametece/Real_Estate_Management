using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstate.Business.Abstract;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;
using RealEstate.Business.Extensions;
using RealEstate.Data.Abstract;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Concrete;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<EProperty> _propertyRepository;
    private readonly IRepository<Inquiry> _inquiryRepository;

    public UserService(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _propertyRepository = _unitOfWork.GetRepository<EProperty>();
        _inquiryRepository = _unitOfWork.GetRepository<Inquiry>();
    }

    public async Task<ResponseDto<UserProfileDto>> GetProfileAsync(int userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
            {
                return ResponseDto<UserProfileDto>.Fail("Kullanıcı bulunamadı", 404);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userProfile = _mapper.Map<UserProfileDto>(user);
            userProfile.Roles = roles.ToList();

            return ResponseDto<UserProfileDto>.Success(userProfile, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<UserProfileDto>.Fail($"Profil getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<UserPagedResultDto<UserListDto>>> GetAllUsersAsync(
        Expression<Func<AppUser, bool>>? predicate = null,
        Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>>? orderBy = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        try
        {
            // Base predicate - soft delete kontrolü
            Expression<Func<AppUser, bool>> basePredicate = x => !x.IsDeleted;
            
            if (predicate != null)
            {
                basePredicate = basePredicate.And(predicate);
            }

            var query = _userManager.Users.Where(basePredicate);

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt);
            }

            var totalCount = await query.CountAsync();
            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserListDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _mapper.Map<UserListDto>(user);
                userDto.Roles = roles.ToList();
                userDtos.Add(userDto);
            }

            var pagedResult = UserPagedResultDto<UserListDto>.Create(userDtos, pageNumber, pageSize, totalCount);
            return ResponseDto<UserPagedResultDto<UserListDto>>.Success(pagedResult, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<UserPagedResultDto<UserListDto>>.Fail($"Kullanıcılar getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<UserDetailDto>> GetUserByIdAsync(int userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
            {
                return ResponseDto<UserDetailDto>.Fail("Kullanıcı bulunamadı", 404);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var propertyCount = await _propertyRepository.CountAsync(x => x.AgentId == userId && !x.IsDeleted);
            var inquiryCount = await _inquiryRepository.CountAsync(x => x.UserId == userId && !x.IsDeleted);

            var userDetail = _mapper.Map<UserDetailDto>(user);
            userDetail.Roles = roles.ToList();
            userDetail.PropertyCount = propertyCount;
            userDetail.InquiryCount = inquiryCount;

            return ResponseDto<UserDetailDto>.Success(userDetail, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<UserDetailDto>.Fail($"Kullanıcı detayı getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> UpdateProfileAsync(int userId, UserUpdateDto updateDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
            {
                return ResponseDto<NoContent>.Fail("Kullanıcı bulunamadı", 404);
            }

            _mapper.Map(updateDto, user);
            user.UpdatedAt = DateTimeOffset.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseDto<NoContent>.Fail($"Profil güncellenemedi: {errors}", 400);
            }

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Profil güncellenirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> UpdateUserRoleAsync(int userId, string roleName)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
            {
                return ResponseDto<NoContent>.Fail("Kullanıcı bulunamadı", 404);
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return ResponseDto<NoContent>.Fail("Geçersiz rol", 400);
            }

            // Mevcut rolleri kaldır
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    return ResponseDto<NoContent>.Fail("Mevcut roller kaldırılamadı", 400);
                }
            }

            // Yeni rol ekle
            var addResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!addResult.Succeeded)
            {
                var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                return ResponseDto<NoContent>.Fail($"Rol atanamadı: {errors}", 400);
            }

            // Agent rolü atanıyorsa IsAgent'ı true yap
            user.IsAgent = roleName.Equals("Agent", StringComparison.OrdinalIgnoreCase);
            user.UpdatedAt = DateTimeOffset.UtcNow;
            await _userManager.UpdateAsync(user);

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Rol güncellenirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> UpdateAgentInfoAsync(int userId, AgentUpdateDto agentUpdateDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
            {
                return ResponseDto<NoContent>.Fail("Kullanıcı bulunamadı", 404);
            }

            if (!user.IsAgent)
            {
                return ResponseDto<NoContent>.Fail("Kullanıcı emlakçı değil", 400);
            }

            _mapper.Map(agentUpdateDto, user);
            user.UpdatedAt = DateTimeOffset.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseDto<NoContent>.Fail($"Emlakçı bilgileri güncellenemedi: {errors}", 400);
            }

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Emlakçı bilgileri güncellenirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<int>> GetUserCountAsync()
    {
        try
        {
            var count = await _userManager.Users.CountAsync(x => !x.IsDeleted);
            return ResponseDto<int>.Success(count, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<int>.Fail($"Kullanıcı sayısı alınırken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<int>> GetAgentCountAsync()
    {
        try
        {
            var count = await _userManager.Users.CountAsync(x => x.IsAgent && !x.IsDeleted);
            return ResponseDto<int>.Success(count, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<int>.Fail($"Emlakçı sayısı alınırken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> SoftDeleteUserAsync(int userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ResponseDto<NoContent>.Fail("Kullanıcı bulunamadı", 404);
            }

            user.IsDeleted = true;
            user.UpdatedAt = DateTimeOffset.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseDto<NoContent>.Fail($"Kullanıcı silinemedi: {errors}", 400);
            }

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Kullanıcı silinirken hata oluştu: {ex.Message}", 500);
        }
    }
}