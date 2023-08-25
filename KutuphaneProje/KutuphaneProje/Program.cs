using KutuphaneProje.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<MyDbContext>();
builder.Services.AddHttpClient();
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddRazorPages();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Admin/Login"; // Giri� yap�lmam��sa y�nlendirilecek sayfa
            options.AccessDeniedPath = "/Home/AccessDenied"; // Yetkisiz eri�im durumunda y�nlendirilecek sayfa
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin")); // "Admin" rol�ne sahip kullan�c�lar i�in
});

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IConfiguration>(configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Admin",
    pattern: "{controller=Admin}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "oku",
    pattern: "{controller=Oku}/{action=Details}/{id?}");

app.MapControllerRoute(
    name: "Yorumlar",
    pattern: "{controller=Api}/{action=Details}/{id?}");

app.MapControllerRoute(
    name: "KitapEkle",
    pattern: "{controller=Admin}/{action=KitapEkle}/{id?}");

app.Run();
