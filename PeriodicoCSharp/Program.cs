using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using DAL.Entidades;
using PeriodicoCSharp.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PeriodicoContext>(
    options => options.UseNpgsql("name=ConnectionStrings:PostgresConnection"));

builder.Services.AddScoped<UsuarioServicio, ImplementacionUsuario>();
builder.Services.AddScoped<InterfazNoticia, ImplementacionNoticia>();
builder.Services.AddScoped<InterfazCategoria, ImplementacionCategoria>();
builder.Services.AddScoped<InterfazEncriptar, ImplementacionEncriptar>();
builder.Services.AddScoped<ConversionDao, ConversionDaoImpl>();
builder.Services.AddScoped<ConversionDTO, ConversionDTOImpl>();
builder.Services.AddScoped<InterfazEmail, ImplementacionEmail>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/auth/login";
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
