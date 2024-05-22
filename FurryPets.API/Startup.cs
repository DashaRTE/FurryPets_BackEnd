using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using FurryPets.Di.DiManagers;
using Microsoft.OpenApi.Models;

namespace FurryPets.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddResponseCaching();
        services.AddResponseCompression();
        services.Configure<GzipCompressionProviderOptions>(static options => options.Level = CompressionLevel.Fastest);

        services.AddCors();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "FurryPets API", Version = "v1" });
        });

        services.AddMvcCore(static mvcOptions => mvcOptions.EnableEndpointRouting = false)
            .AddApiExplorer();

        services.AddMapper();

        services.AddTransient<HttpClient>();

        services.AddData(Configuration.GetConnectionString("DefaultConnection")!)
            .AddUseCases()
            .AddServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsProduction())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(static swaggerUiOptions => swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "FurryPets API"));
        app.UseStaticFiles();

        app.UseCors(static corsPolicyBuilder => corsPolicyBuilder
            .WithOrigins("http://localhost:4000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());

        app.UseRouting();

        app.UseEndpoints(static endpoints => endpoints.MapControllers());
    }
}
