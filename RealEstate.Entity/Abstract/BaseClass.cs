using System;

namespace RealEstate.Entity.Abstract;

public class BaseClass
{
     public int Id { get; set; }

     public bool IsDeleted { get; set; }  = false;
     
     public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow ;

     public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow ;

     

}
