using Microsoft.EntityFrameworkCore;
using NWSDB.BusinessLogic.Interfaces;
using NWSDB.BusinessLogic.Services;
using NWSDB.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession();

// Connect WebApp directly to NWSDB_Dev Database
builder.Services.AddDbContext<NwsdbContext>(options =>
    options.UseSqlServer(
        "Server=(localdb)\\mssqllocaldb;Database=NWSDB_Dev;Trusted_Connection=True;MultipleActiveResultSets=true"));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILoginService, LoginService>();

// Connect WebApp to NWSDB API
builder.Services.AddHttpClient("NwsdbApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7167/");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();