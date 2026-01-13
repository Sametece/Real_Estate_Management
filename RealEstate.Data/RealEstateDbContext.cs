using System;
using Microsoft.EntityFrameworkCore;
using RealEstate.Entity.Concrete;
using RealEstate.Entity.Enum;

namespace RealEstate.Data;

public class RealEstateDbContext : DbContext
{
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options):base(options)
    {
        
    }
   public DbSet<EProperty> eProperties { get; set; } 
   public DbSet<Inquiry> Inquiries {get; set;}

   public DbSet<PropertyType> propertyTypes {get; set;}

   public DbSet<PropertyImage> propertyImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EProperty>()
                                       .HasQueryFilter(x => !x.IsDeleted); // Soft Delete için
   
      #region Emlak Tipleri
             
             var propertyType = new PropertyType []
             {
                 new PropertyType { Id =1 , Name = "Daire", Description = "Satılık Daire ", CreatedAt =new DateTime(2026,01,09)},
                 new PropertyType { Id = 2, Name = "Daire", Description= "Kiralık Daire", CreatedAt =new DateTime(2026,01,09)  },
                 new PropertyType{ Id = 3, Name = "Villa", Description= "Kiralık Villa", CreatedAt =new DateTime(2026,01,09)  },
                 new PropertyType{Id = 4, Name = "Dükkan", Description= "Satılık Dükkan", CreatedAt =new DateTime(2026,01,09) },
                 new PropertyType{Id = 5, Name = "Ofis", Description= "Satılık Ofis", CreatedAt =new DateTime(2026,01,09)    },
                 new PropertyType{ Id = 6, Name = "Arsa", Description= "Satılık Arsa", CreatedAt =new DateTime(2026,01,09)   },
                 new PropertyType{Id = 7, Name = "İşyeri", Description= "Kiralık İşyeri", CreatedAt =new DateTime(2026,01,09)}
             };


             modelBuilder.Entity<PropertyType>().HasData(propertyTypes);


        #endregion


        #region Emlak İlanı

        var  eProperty = new EProperty []
        {
            new EProperty
            {
        Id = 1,
        Title = "Deniz Manzaralı 3+1 Satılık Daire",
        Description = "Adalar manzaralı, geniş ve ferah satılık daire",
        Price = 3_250_000,
        Address = "Backend Mahallesi",
        City = "İstanbul",
        District = "Kartal",
        Rooms = 3,
        Area = 135,
        YearBuilt = 2018,
        Floor = 5,
        Status = PropertyStatus.Available,
        PropertyTypeId = 1, // Satılık Daire
        CreatedAt = new DateTime(2026,01,09),
        IsDeleted = false
         },
           new EProperty
          {
        Id = 2,
        Title = "Deniz Manzaralı 1+1 Kiralık Daire",
        Description = "Site içerisinde, güvenlikli kiralık daire",
        Price = 18_000,
        Address = "Backend Mahallesi",
        City = "İstanbul",
        District = "Maltepe",
        Rooms = 1,
        Area = 60,
        YearBuilt = 2020,
        Floor = 3,
        Status = PropertyStatus.Rented,
        PropertyTypeId = 2, // Kiralık Daire
        CreatedAt = new DateTime(2026,01,09),
        IsDeleted = false
         },
           new EProperty
         {
        Id = 3,
        Title = "Bahçeli Kiralık Villa",
        Description = "Müstakil bahçeli, havuzlu villa",
        Price = 55_000,
        Address = "Villa Sokak",
        City = "İstanbul",
        District = "Beykoz",
        Rooms = 1,
        Area = 60,
        YearBuilt = 2020,
        Floor = 3,
        Status = PropertyStatus.Rented,
        PropertyTypeId = 3, // Kiralık Villa
        CreatedAt = new DateTime(2026,01,09),
        IsDeleted = false
         },
          new EProperty
          {
        Id = 4,
        Title = "Merkezi Konumda Satılık Dükkan",
        Description = "Cadde üzerinde, yüksek yaya trafiği",
        Price = 6_500_000,
        Address = "Çarşı Caddesi",
        City = "İstanbul",
        District = "Kadıköy",
         Rooms = 1,
        Area = 60,
        YearBuilt = 2024,
        Floor = 3,
        Status = PropertyStatus.Sold,
        PropertyTypeId = 4, // Satılık Dükkan
        CreatedAt = new DateTime(2026,01,09),
        IsDeleted = false
         }
         };


       modelBuilder.Entity<EProperty>().HasData(eProperties);
   
        #endregion
       
        #region Emlak Resimleri
         
         modelBuilder.Entity<PropertyImage>().HasData(
        new PropertyImage
        {
           Id =1,
           ImageUrl = "https://example.com/property1-1.jpg",
            IsPrimary= true,
           PropertyId = 1 ,
            CreatedAt = new DateTime(2026,01,09)
           
        },
        new PropertyImage
        {
           Id =2,
           ImageUrl = "https://example.com/property1-2.jpg",
           IsPrimary= true,
           PropertyId = 1,
           CreatedAt = new DateTime(2026,01,09)
        },
         new PropertyImage
        {
            Id = 3,
            ImageUrl = "https://example.com/property2-1.jpg",
            IsPrimary= true,
            PropertyId = 2,
            CreatedAt = new DateTime(2026,01,09)
        },
         new PropertyImage
        {
            Id = 4,
            ImageUrl = "https://example.com/property2-2.jpg",
            IsPrimary= true,
            PropertyId = 2,
            CreatedAt = new DateTime(2026,01,09)
        }
        );

        #endregion
   
   
        #region İletişim


        modelBuilder.Entity<Inquiry>().HasData(
         
          new Inquiry
          {
             Id =1,
            Name = "Samet Ece",
            Email = "SametEce@Backend.com",
            Message = "Bu ilan hakkında bilgi almak istiyorum.",
            Status = InquiryStatus.New,
            PropertyId = 1,
            CreatedAt = new DateTime(2026,01,09)


          },
          new Inquiry
          {
             Id =2,
            Name = "Ece",
            Email = "Ece@Backend.com",
            Message = "Kira Şartları Nedir?",
            Status = InquiryStatus.Contacted,
            PropertyId = 2,
            CreatedAt = new DateTime(2026,01,09)
          }


        );
           
        #endregion

           

            
       
    }
}
