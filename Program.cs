using App_practical.Models;
using App_practical.Services; // Necesario para encontrar tu KafkaProducerService
using Microsoft.EntityFrameworkCore;
using Confluent.Kafka;        // Necesario para el tipo Null de Kafka

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// 👇 AQUÍ ESTÁ LA LÍNEA MÁGICA QUE SOLUCIONA EL ERROR 👇
builder.Services.AddSingleton<KafkaProducerService<Null, string>>();

builder.Services.AddDbContext<DatabaseContext>(
    o => o.UseNpgsql("Host=postgres;Port=5432;Database=Calculator;Username=postgres;Password=admin")
);

var app = builder.Build();

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
            await Task.Delay(3000);
        }
    }
}
