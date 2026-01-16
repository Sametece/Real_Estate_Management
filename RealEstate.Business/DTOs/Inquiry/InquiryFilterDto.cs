using System;
using RealEstate.Entity.Enum;

namespace RealEstate.Business.DTOs.Inquiry;

public class InquiryFilterDto
{
public int? EPropertyId { get; set; }

 public string? Email { get; set; }

public InquiryStatus? Status { get; set; }

public DateTime? StartDate { get; set; }
public DateTime? EndDate { get; set; }


}
