using System;
using AutoMapper;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.Inquiry;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Mapping;

public class InquiryProfile : Profile
{
  public InquiryProfile()
    {
        //Create
         
         CreateMap<InquiryCreateDto, Inquiry>();
        
        
        //update
        
        CreateMap<InquiryAdminUpdateDto, Inquiry>();

        //List
         
         CreateMap<Inquiry, InquiryListDto>()
            .ForMember(dest => dest.PropertyTitle,
                       opt => opt.MapFrom(src => src.Property.Title));


        //Detail

        CreateMap<Inquiry, InquiryDeatilDto>()
            .ForMember(dest => dest.PropertyTitle,
                       opt => opt.MapFrom(src => src.Property.Title));
    }
}
