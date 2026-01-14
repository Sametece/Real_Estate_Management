using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstate.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "propertyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_propertyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    District = table.Column<string>(type: "TEXT", nullable: true),
                    Rooms = table.Column<int>(type: "INTEGER", nullable: false),
                    Bathrooms = table.Column<int>(type: "INTEGER", nullable: true),
                    Area = table.Column<decimal>(type: "TEXT", nullable: false),
                    Floor = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalFloors = table.Column<int>(type: "INTEGER", nullable: true),
                    YearBuilt = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    AgentId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_eProperties_propertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "propertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inquiries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inquiries_eProperties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "eProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "propertyImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_propertyImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_propertyImages_eProperties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "eProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "propertyTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Satılık Daire ", false, "Daire", new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 559, DateTimeKind.Unspecified).AddTicks(658), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Kiralık Daire", false, "Daire", new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 561, DateTimeKind.Unspecified).AddTicks(3999), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Kiralık Villa", false, "Villa", new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 561, DateTimeKind.Unspecified).AddTicks(4014), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Satılık Dükkan", false, "Dükkan", new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 561, DateTimeKind.Unspecified).AddTicks(4016), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 5, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Satılık Ofis", false, "Ofis", new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 561, DateTimeKind.Unspecified).AddTicks(4018), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 6, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Satılık Arsa", false, "Arsa", new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 561, DateTimeKind.Unspecified).AddTicks(4020), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 7, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Kiralık İşyeri", false, "İşyeri", new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 561, DateTimeKind.Unspecified).AddTicks(4022), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "eProperties",
                columns: new[] { "Id", "Address", "AgentId", "Area", "Bathrooms", "City", "CreatedAt", "Description", "District", "Floor", "IsDeleted", "Price", "PropertyTypeId", "Rooms", "Status", "Title", "TotalFloors", "UpdatedAt", "YearBuilt" },
                values: new object[,]
                {
                    { 1, "Backend Mahallesi", 1, 135m, null, "İstanbul", new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Adalar manzaralı, geniş ve ferah satılık daire", "Kartal", 5, false, 3250000m, 1, 3, 1, "Deniz Manzaralı 3+1 Satılık Daire", null, new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 562, DateTimeKind.Unspecified).AddTicks(1420), new TimeSpan(0, 0, 0, 0, 0)), 2018 },
                    { 2, "Backend Mahallesi", 2, 60m, null, "İstanbul", new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Site içerisinde, güvenlikli kiralık daire", "Maltepe", 3, false, 18000m, 2, 1, 4, "Deniz Manzaralı 1+1 Kiralık Daire", null, new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 562, DateTimeKind.Unspecified).AddTicks(6364), new TimeSpan(0, 0, 0, 0, 0)), 2020 },
                    { 3, "Villa Sokak", 2, 60m, null, "İstanbul", new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Müstakil bahçeli, havuzlu villa", "Beykoz", 3, false, 55000m, 3, 1, 4, "Bahçeli Kiralık Villa", null, new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 562, DateTimeKind.Unspecified).AddTicks(6374), new TimeSpan(0, 0, 0, 0, 0)), 2020 },
                    { 4, "Çarşı Caddesi", 3, 60m, null, "İstanbul", new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Cadde üzerinde, yüksek yaya trafiği", "Kadıköy", 3, false, 6500000m, 4, 1, 3, "Merkezi Konumda Satılık Dükkan", null, new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 562, DateTimeKind.Unspecified).AddTicks(6378), new TimeSpan(0, 0, 0, 0, 0)), 2024 }
                });

            migrationBuilder.InsertData(
                table: "Inquiries",
                columns: new[] { "Id", "CreatedAt", "Email", "IsDeleted", "Message", "Name", "Phone", "PropertyId", "Status", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "SametEce@Backend.com", false, "Bu ilan hakkında bilgi almak istiyorum.", "Samet Ece", null, 1, 1, new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 562, DateTimeKind.Unspecified).AddTicks(8750), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 2, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), "Ece@Backend.com", false, "Kira Şartları Nedir?", "Ece", null, 2, 2, new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 563, DateTimeKind.Unspecified).AddTicks(479), new TimeSpan(0, 0, 0, 0, 0)), null }
                });

            migrationBuilder.InsertData(
                table: "propertyImages",
                columns: new[] { "Id", "CreatedAt", "DisplayOrder", "ImageUrl", "IsDeleted", "IsPrimary", "PropertyId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), 0, "https://example.com/property1-1.jpg", false, true, 1, new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 562, DateTimeKind.Unspecified).AddTicks(7102), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 2, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), 0, "https://example.com/property1-2.jpg", false, true, 1, new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 562, DateTimeKind.Unspecified).AddTicks(8091), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 3, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), 0, "https://example.com/property2-1.jpg", false, true, 2, new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 562, DateTimeKind.Unspecified).AddTicks(8093), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4, new DateTimeOffset(new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 3, 0, 0, 0)), 0, "https://example.com/property2-2.jpg", false, true, 2, new DateTimeOffset(new DateTime(2026, 1, 14, 10, 8, 11, 562, DateTimeKind.Unspecified).AddTicks(8095), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_eProperties_PropertyTypeId",
                table: "eProperties",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_PropertyId",
                table: "Inquiries",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_propertyImages_PropertyId",
                table: "propertyImages",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inquiries");

            migrationBuilder.DropTable(
                name: "propertyImages");

            migrationBuilder.DropTable(
                name: "eProperties");

            migrationBuilder.DropTable(
                name: "propertyTypes");
        }
    }
}
