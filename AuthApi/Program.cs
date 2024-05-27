using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthApi.Data;
using AuthApi.Models;
using AuthApi.Interface;
using AuthAPI;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
});

// Configure DbContext for AuthService to use SQLite
builder.Services.AddDbContext<AuthContext>(options =>
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

// Register the AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Add endpoints for AuthService
app.MapPost("/api/auth/register", async (RegisterModel model, IAuthService authService) =>
{
    var result = await authService.RegisterAsync(model);
    if (!result.Success)
    {
        return Results.BadRequest(result.Message);
    }
    return Results.Ok(result);
});

app.MapPost("/api/auth/login", async (LoginModel model, IAuthService authService) =>
{
    var result = await authService.LoginAsync(model);
    if (!result.Success)
    {
        return Results.Unauthorized();
    }
    return Results.Ok(result);
});

app.MapGet("/api/auth/exists/{username}", async (string username, IAuthService authService) =>
{
    var exists = await authService.UserExistsAsync(username);
    return Results.Ok(new { exists });
});



app.Run();