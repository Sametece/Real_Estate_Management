using System;

namespace RealEstate.Business.DTOs.Inquiry;

public class InquiryDeatilDto
{
  //admin ve emlakçı için detay ekranı 

    public int Id { get; set; }

    public int EPropertyId { get; set; }
    public string PropertyTitle { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

}
