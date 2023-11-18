using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using FurryPets.Core.Enumerations;
using FurryPets.Shared;
using FurryPets.Shared.Auth;
using FurryPets.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FurryPets.Api.Configuration;
using FurryPets.Di.DiManagers;

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

        services.AddMvcCore(static mvcOptions => mvcOptions.EnableEndpointRouting = false)
            .AddAuthorization(static options =>
            {
                options.AddPolicy(RoleType.User.ToString(), static policy => policy.Requirements.Add(new RoleRequirement(RoleType.User.ToString())));
            })
            .ConfigureApiBehaviorOptions(static options =>
                options.InvalidModelStateResponseFactory = static actionContext =>
                {
                    var modelState = actionContext.ModelState;

                    return new BadRequestObjectResult(FormatOutput(modelState));
                })
            .AddDataAnnotations()
            .AddApiExplorer();

        services.AddMapper()
            .AddIdentity()
            .AddJwtAuthentication(Configuration)
            .AddSingleton<IAuthorizationHandler, RoleHandler>()
            .AddSwagger();

        services.AddTransient<HttpClient>();

        services.AddData(Configuration.GetConnectionString("DefaultConnection")!)
            .AddJwt()
            .AddUseCases()
            .AddServices()
            .AddOptions(Configuration);
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(static endpoints => endpoints.MapControllers());
    }

    private static ValidationResultModel FormatOutput(ModelStateDictionary modelState)
    {
        var result = new ValidationResultModel(modelState.Where(static modelStateEntry => modelStateEntry.Value?.ValidationState == ModelValidationState.Invalid)
            .Select(static modelStateEntry =>
                $"{modelStateEntry.Key.ToLowerFirstChar()}: {string.Join(" ", modelStateEntry.Value!.Errors.Select(static error => error.ErrorMessage))}"));

        return result;
    }
}
