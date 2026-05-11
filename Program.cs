using OpenTelemetry.Metrics;

using OpenTelemetry.Metrics; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();


builder.Services.AddOpenTelemetry()
    .WithMetrics(meterProviderBuilder =>
    {
        meterProviderBuilder.AddPrometheusExporter(); 
        meterProviderBuilder.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel"); 
        meterProviderBuilder.AddMeter("Microsoft.AspNetCore.Http.Connections"); 


meterProviderBuilder.AddView("http.server.request.duration",
            new ExplicitBucketHistogramConfiguration
            {
                Boundaries = [0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10]
            });
    });

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapPrometheusScrapingEndpoint(); 
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();