using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting;
using Proyect_two;
using Proyect_two.Data;
using Proyect_two.Pages.Clases_Utiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<UsuarioService>(); // Registra el servicio UsuarioService

// Obtén la cadena de conexión desde la configuración
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra el servicio SolicitudService con la cadena de conexión
builder.Services.AddScoped<SolicitudService>(serviceProvider => new SolicitudService(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
