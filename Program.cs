using Microsoft.EntityFrameworkCore;
using simplified_picpay.Data;
using simplified_picpay.Repositories;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services;
using simplified_picpay.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new Exception("Connection string não encontrada!");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
