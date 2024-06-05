using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotesService.Context;
using NotesService.Interface;
using NotesService.Class;
using NotesService.Controllers;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notes API", Version = "v1" });
});

// Configure DbContext for NotesService to use SQLite
builder.Services.AddDbContext<NotesContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the NotesService
builder.Services.AddScoped<INotesService, NotesServiceClass>();

// Register the AuthService and PasswordManagerService
//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IPasswordManagerService, PasswordManagerService>();
var options = new JsonSerializerOptions()
{
    AllowTrailingCommas = true
};
var app = builder.Build();

//app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notes API v1");
    });
}

app.UseHttpsRedirection();

// Add endpoints for all services
app.MapControllers();

app.Run();
