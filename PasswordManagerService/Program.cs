using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PasswordManagerService.Class;
using PasswordManagerService.Data;
using PasswordManagerService.Interface;
using PasswordManagerService.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
//builder.AddServiceDefaults();
var options = new JsonSerializerOptions()
{
    AllowTrailingCommas = true
};
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Password Manager API", Version = "v1" });
});

// Configure DbContext for PasswordManagerService to use SQLite
builder.Services.AddDbContext<PasswordManagerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add authentication and JWT services
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
        ValidIssuer = "yourissuer",
        ValidAudience = "youraudience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yoursecretkey"))
    };
});

// Add authorization services
builder.Services.AddAuthorization();

// Register the PasswordManagerService
builder.Services.AddScoped<IPasswordManagerService, PasswordManagerServiceClass>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

//app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Password Manager API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Add endpoints for PasswordManagerService
app.MapPost("/api/passwords/store", async (PasswordModel model, IPasswordManagerService passwordManagerService) =>
{
    var result = await passwordManagerService.StorePasswordAsync(model);
    if (!result.Success)
    {
        return Results.BadRequest(result.Message);
    }
    return Results.Ok(result);
});

app.MapGet("/api/passwords/retrieve", async (string username, IPasswordManagerService passwordManagerService) =>
{
    var passwords = await passwordManagerService.RetrievePasswordsAsync(username);
    if (passwords == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(passwords);
});

app.Run();
