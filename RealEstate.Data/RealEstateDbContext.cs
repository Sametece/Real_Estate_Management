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
   public DbSet<EProperty> Properties { get; set; } 
   public DbSet<Inquiry> Inquiries {get; set;}

   public DbSet<PropertyType> propertyTypes {get; set;}

   public DbSet<PropertyImage> propertyImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EProperty>().HasQueryFilter(x=> !x.IsDeleted);
        modelBuilder.Entity<>
    }
}

