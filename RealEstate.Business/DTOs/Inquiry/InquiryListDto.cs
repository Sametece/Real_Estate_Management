using System;

namespace RealEstate.Business.DTOs.Inquiry;

public class InquiryListDto
{
   //Admin ve emlakçı mesaj listeleme ekranı

   public int Id { get; set; }

   public int EPropertyId { get; set; }

   public string PropertyTitle { get; set; } = null!;

   public string Name { get; set; } = null!;

   public string Email { get; set; } = null!;

   public DateTime CreatedAt { get; set; }
}
