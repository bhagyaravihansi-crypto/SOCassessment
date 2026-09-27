using Microsoft.EntityFrameworkCore;
using NWSDB.DataAccess;
using NWSDB.BusinessLogic.Interfaces;
using NWSDB.BusinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container


builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// Database


builder.Services.AddDbContext<NwsdbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "NwsdbDevConnection")));


// BusinessLogic Services


builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IBillService, BillService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IUsageService, UsageService>();


// Build application


var app = builder.Build();

// Configure HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();