using Microsoft.EntityFrameworkCore;
using TradingApp.Data;
using TradingApp.Helpers.Controllers;
using TradingApp.Helpers.Services;
using TradingApp.Models.Interfaces;
using TradingApp.Models.Interfaces.ControllerHelpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TradingAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Helpers
builder.Services.AddScoped<IAccountControllerHelper, AccountControllerHelper>();
builder.Services.AddScoped<ISecurityControllerHelper, SecurityControllerHelper>();
builder.Services.AddScoped<ITransactionControllerHelper, TransactionControllerHelper>();
builder.Services.AddScoped<ISecurityPricesControllerHelper, SecurityPricesControllerHelper>();
builder.Services.AddScoped<IHoldingsControllerHelper, HoldingsControllerHelper>();
builder.Services.AddScoped<IPerformanceControllerHelper, PerformanceControllerHelper>();

// Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
builder.Services.AddScoped<ISecurityPriceService, SecurityPriceService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "https://orange-grass-005965103.7.azurestaticapps.net"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWebApp");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();