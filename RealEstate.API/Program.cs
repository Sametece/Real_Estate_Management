using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstate.API.Middleware;
using RealEstate.Business.Abstract;
using RealEstate.Business.Concrete;
using RealEstate.Business.Configuration;
using RealEstate.Business.Mapping;
using RealEstate.Business.Validators;
using RealEstate.Data;
using RealEstate.Data.Abstract;
using RealEstate.Data.Concrete;
using RealEstate.Entity.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'SqliteConnection' bulunamadı!");
}
builder.Services.AddDbContext<RealEstateDbContext>(options => options.UseSqlite(connectionString));

// Identity Configuration
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<RealEstateDbContext>()
.AddDefaultTokenProviders();

// JWT Configuration
var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
if (jwtConfig == null)
{
    throw new InvalidOperationException("JWT configuration bulunamadı!");
}
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Repository and UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Business Services
builder.Services.AddScoped<IEPropertyService, EPropertyService>();
builder.Services.AddScoped<IPropertyTypeService, PropertyTypeService>();
builder.Services.AddScoped<IInguiryService, InquiryService>();
builder.Services.AddScoped<IPropertyImageService, PropertyImageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICacheService, CacheService>();

// Memory Cache
builder.Services.AddMemoryCache();

// AutoMapper
builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<EPropertyProfile>();
    cfg.AddProfile<InquiryProfile>();
    cfg.AddProfile<PropertyTypeProfile>();
    cfg.AddProfile<PropertyImageProfile>();
    cfg.AddProfile<UserProfile>();
});

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<PropertyCreateDtoValidator>();

// Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Real Estate API V1");
    });
}

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

// Global Exception Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Response Compression
app.UseResponseCompression();

app.UseHttpsRedirection();

// CORS
app.UseCors("AllowAll");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Seed data oluşturulurken hata oluştu");
    }
}

app.Run();
