using System;

namespace RealEstate.Business.DTOs;

public class InquiryCreateDto
{ 

  //User Bir ilana mesaj atarken
  public int EPropertyId { get; set; }
  public string Name { get; set; } = null!;

  public string Email { get; set; } = null!;

  public string  Message { get; set; } = null!;
}
