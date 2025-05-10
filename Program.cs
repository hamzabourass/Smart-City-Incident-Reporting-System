using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SCIRS.Data;
using SCIRS.Models;
using SCIRS.Profiles;
using SCIRS.Repository.Implementations;
using SCIRS.Repository.Interfaces;
using SCIRS.Services.Implementations;
using SCIRS.Services.Interfaces;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ScirsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SCIRSDb"),
    x => x.UseNetTopologySuite()
    ));

builder.Services.AddAutoMapper(config => {
    config.AddProfile<MappingProfile>();
});

// Repositories
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ScirsContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var jwtConfig = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["SecretKey"]))
    };
});


var app = builder.Build();

await app.MigrateDbAsync();
await app.SeedTestDataAsync();


// Configure the HTTP request pipeline.

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
   options.ConfigObject.Urls = [new UrlDescriptor { Name = "SCIRS v1", Url = "/openapi/v1.json" }]; ;
});



app.UseCors("AllowAll");

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

