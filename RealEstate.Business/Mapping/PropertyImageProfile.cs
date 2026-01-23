using System;
using AutoMapper;
using RealEstate.Business.DTOs;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Mapping;

public class PropertyImageProfile : Profile
{
   public PropertyImageProfile()
    {
        

        //List - Create

        CreateMap<PropertyImage, PropertyImageDto>();
    }
}
