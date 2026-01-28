using System;
using System.Linq.Expressions;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Abstract;

public interface IUserService
{
    /// <summary>
    /// Kullanıcı profil bilgilerini getir
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ResponseDto<UserProfileDto>> GetProfileAsync(int userId);

    /// <summary>
    /// Tüm kullanıcıları listele (Admin)
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<ResponseDto<UserPagedResultDto<UserListDto>>> GetAllUsersAsync(
        Expression<Func<AppUser, bool>>? predicate = null,
        Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>>? orderBy = null,
        int pageNumber = 1,
        int pageSize = 10
    );

    /// <summary>
    /// Kullanıcı detayını getir (Admin)
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ResponseDto<UserDetailDto>> GetUserByIdAsync(int userId);

    /// <summary>
    /// Kullanıcı profilini güncelle
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="updateDto"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> UpdateProfileAsync(int userId, UserUpdateDto updateDto);

    /// <summary>
    /// Kullanıcı rolünü güncelle (Admin)
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> UpdateUserRoleAsync(int userId, string roleName);

    /// <summary>
    /// Emlakçı bilgilerini güncelle
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="agentUpdateDto"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> UpdateAgentInfoAsync(int userId, AgentUpdateDto agentUpdateDto);

    /// <summary>
    /// Kullanıcı sayısını getir
    /// </summary>
    /// <returns></returns>
    Task<ResponseDto<int>> GetUserCountAsync();

    /// <summary>
    /// Emlakçı sayısını getir
    /// </summary>
    /// <returns></returns>
    Task<ResponseDto<int>> GetAgentCountAsync();

    /// <summary>
    /// Kullanıcıyı soft delete
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> SoftDeleteUserAsync(int userId);
}