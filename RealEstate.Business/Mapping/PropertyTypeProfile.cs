using System;
using AutoMapper;
using RealEstate.Business.DTOs.PropertyTypeDto;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Mapping;

public class PropertyTypeProfile : Profile
{
   public PropertyTypeProfile()
    {
        //Create 

        CreateMap<PropertyTypeCreateDto , PropertyType>();

        // Update

        CreateMap<PropertyTypeUpdateDto, PropertyType>();

        //List 

        CreateMap<PropertyType , PropertyTypeListDto>();
    }
}
