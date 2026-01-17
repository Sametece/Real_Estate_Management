using System;
using AutoMapper;
using RealEstate.Business.DTOs;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Mapping;

public class EPropertyProfile : Profile
{
   public EPropertyProfile()
    {
      
        // CREATE
        
        CreateMap<EPropertyCreateDto, EProperty>();

       
        // UPDATE (Admin)
     
        CreateMap<EPropertyAdminUpdateDto, EProperty>()
            .ForAllMembers(
                opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));

        
        // UPDATE (Agent)
       
        CreateMap<EPropertyAgentUpdateDto, EProperty>()
            .ForAllMembers(
                opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));

       
        // LIST
    
        CreateMap<EProperty, EPropertyListDto>()
            .ForMember(dest => dest.PropertyTypeName,
                       opt => opt.MapFrom(src => src.PropertyType.Name))
            .ForMember(dest => dest.CoverImage,
                       opt => opt.MapFrom(src =>
                           src.Images.FirstOrDefault(i => i.IsPrimary)!.ImageUrl));

     
        // DETAIL
      
        CreateMap<EProperty, EPropertyDetailDto>()
            .ForMember(dest => dest.PropertyTypeName,
                       opt => opt.MapFrom(src => src.PropertyType.Name))
            .ForMember(dest => dest.Images,
                       opt => opt.MapFrom(src => src.Images));
    }
}
