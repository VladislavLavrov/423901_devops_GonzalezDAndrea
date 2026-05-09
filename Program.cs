using App_practical.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Configuración de la conexión a PostgreSQL
builder.Services.AddDbContext<DatabaseContext>(
    o => o.UseNpgsql("Host=postgres;Port=5432;Database=Calculator;Username=postgres;Password=admin")
);

var app = builder.Build();

// Lógica para crear la base de datos automáticamente al iniciar
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    await WaitForDatabase(context);
    await context.Database.EnsureCreatedAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Función para esperar a que Postgres esté listo en el contenedor
async Task WaitForDatabase(DatabaseContext context)
{
    int retries = 10;
    while (retries > 0)
    {
        try
        {
            if (await context.Database.CanConnectAsync()) return;
        }
        catch
        {
            retries--;
            await Task.Delay(2000);
        }
    }
}