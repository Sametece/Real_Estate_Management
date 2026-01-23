using Microsoft.EntityFrameworkCore;
using RealEstate.Business.Mapping;
using RealEstate.Data;
using RealEstate.Data.Abstract;
using RealEstate.Data.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Db Ayarları
var connectionString = builder.Configuration.GetConnectionString("SqliteConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'SqliteConnection' bulunamadı!");
}
builder.Services.AddDbContext<RealEstateDbContext>(options => options.UseSqlite(connectionString));

//Merkezi Kaydetme
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

//mapleme

builder.Services.AddAutoMapper(typeof(EPropertyProfile).Assembly);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
