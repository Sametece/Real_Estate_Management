using AutoMapper;
using RealEstate.Business.DTOs;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, UserProfileDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles will be set manually

        CreateMap<AppUser, UserListDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles will be set manually

        CreateMap<AppUser, UserDetailDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore()) // Roles will be set manually
            .ForMember(dest => dest.PropertyCount, opt => opt.Ignore()) // Will be set manually
            .ForMember(dest => dest.InquiryCount, opt => opt.Ignore()); // Will be set manually

        CreateMap<UserUpdateDto, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.IsAgent, opt => opt.Ignore());

        CreateMap<AgentUpdateDto, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.Ignore())
            .ForMember(dest => dest.LastName, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.IsAgent, opt => opt.Ignore());
    }
}