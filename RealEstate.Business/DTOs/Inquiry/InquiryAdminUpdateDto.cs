using System;
using RealEstate.Entity.Enum;

namespace RealEstate.Business.DTOs.Inquiry;

public class InquiryAdminUpdateDto
{  
    //Durum yönetimi için 1-2-3-4
   public InquiryStatus Status { get; set; }
}
