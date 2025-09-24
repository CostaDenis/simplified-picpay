using Microsoft.EntityFrameworkCore;
using simplified_picpay.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new Exception("Connection string não encontrada!");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
