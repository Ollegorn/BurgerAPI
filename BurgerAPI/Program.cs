using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using RepositoryContracts;
using ServiceContracts.Interfaces;
using Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<BurgerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBurgerRepository, BurgerRepository>();
builder.Services.AddScoped<IBurgerGetterService, BurgerGetterService>();
builder.Services.AddScoped<IBurgerAdderService, BurgerAdderService>();
builder.Services.AddScoped<IBurgerUpdaterService, BurgerUpdaterService>();
builder.Services.AddScoped<IBurgerDeleterService, BurgerDeleterService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
}).AddEntityFrameworkStores<BurgerDbContext>().AddDefaultTokenProviders();

var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]);

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateAudience = false,
    ValidAudience = builder.Configuration["JwtSettings:Audience"],
    ValidateIssuer = false,
    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
    RequireExpirationTime = true, //update when add refresh token
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key)
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
});


builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
