using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RealEstate.Entity.Concrete;
using RealEstate.Entity.Enum;

namespace RealEstate.Data;

public class RealEstateDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options)
    {
    }

    public DbSet<EProperty> EProperties { get; set; }
    public DbSet<Inquiry> Inquiries { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global Query Filters for Soft Delete
        modelBuilder.Entity<EProperty>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<PropertyType>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<PropertyImage>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Inquiry>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<RefreshToken>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<AppUser>().HasQueryFilter(x => !x.IsDeleted);

        // Entity Configurations
        ConfigureEProperty(modelBuilder);
        ConfigurePropertyType(modelBuilder);
        ConfigurePropertyImage(modelBuilder);
        ConfigureInquiry(modelBuilder);
        ConfigureRefreshToken(modelBuilder);
        ConfigureAppUser(modelBuilder);
    }

    private void ConfigureEProperty(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EProperty>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(5000);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Area).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.District).HasMaxLength(100);

            // Relationships
            entity.HasOne(e => e.PropertyType)
                  .WithMany(pt => pt.EProperties)
                  .HasForeignKey(e => e.PropertyTypeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Images)
                  .WithOne(pi => pi.Property)
                  .HasForeignKey(pi => pi.PropertyId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Inquiries)
                  .WithOne(i => i.Property)
                  .HasForeignKey(i => i.PropertyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigurePropertyType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.HasKey(pt => pt.Id);
            entity.Property(pt => pt.Name).IsRequired().HasMaxLength(100);
            entity.Property(pt => pt.Description).HasMaxLength(500);
        });
    }

    private void ConfigurePropertyImage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyImage>(entity =>
        {
            entity.HasKey(pi => pi.Id);
            entity.Property(pi => pi.ImageUrl).IsRequired().HasMaxLength(1000);
        });
    }

    private void ConfigureInquiry(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inquiry>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Email).IsRequired().HasMaxLength(255);
            entity.Property(i => i.Phone).HasMaxLength(20);
            entity.Property(i => i.Message).IsRequired().HasMaxLength(1000);
        });
    }

    private void ConfigureRefreshToken(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(rt => rt.Id);
            entity.Property(rt => rt.Token).IsRequired().HasMaxLength(500);
            entity.Property(rt => rt.ReasonRevoked).HasMaxLength(200);
            entity.Property(rt => rt.ReplacedByToken).HasMaxLength(500);

            entity.HasOne(rt => rt.User)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(rt => rt.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureAppUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
            entity.Property(u => u.ProfilePicture).HasMaxLength(1000);
            entity.Property(u => u.AgencyName).HasMaxLength(200);
            entity.Property(u => u.LicenseNumber).HasMaxLength(50);

            entity.HasMany(u => u.Properties)
                  .WithOne()
                  .HasForeignKey(p => p.AgentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.Inquiries)
                  .WithOne()
                  .HasForeignKey(i => i.UserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
