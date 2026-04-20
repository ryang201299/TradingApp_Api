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
    options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=TradingApp;Trusted_Connection=True;TrustServerCertificate=True;")
);

// Helpers
builder.Services.AddScoped<IAccountControllerHelper, AccountControllerHelper>();
builder.Services.AddScoped<ISecurityControllerHelper, SecurityControllerHelper>();
builder.Services.AddScoped<ITransactionControllerHelper, TransactionControllerHelper>();
builder.Services.AddScoped<ISecurityPricesControllerHelper, SecurityPricesControllerHelper>();

// Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
builder.Services.AddScoped<ISecurityPriceService, SecurityPriceService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();