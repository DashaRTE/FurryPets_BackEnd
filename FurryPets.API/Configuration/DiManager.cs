using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using FurryPets.Core.Interfaces;
using FurryPets.Infrastructure.Auth;
using FurryPets.Shared;

namespace FurryPets.Api.Configuration;

public static class DiManager
{
	internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(static cfg =>
			{
				cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(cfg =>
			{
#if DEBUG
				cfg.RequireHttpsMetadata = false;
#endif
				cfg.SaveToken = true;
				cfg.Audience = configuration.GetSection("JwtIssuerOptions:Audience").Value;
				cfg.ClaimsIssuer = configuration.GetSection("JwtIssuerOptions:Issuer").Value;

				cfg.TokenValidationParameters = new()
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtIssuerOptions:SecretKey").Value!)),
					ValidateIssuer = true,
					ValidIssuer = configuration.GetSection("JwtIssuerOptions:Issuer").Value,
					ValidateAudience = true,
					ValidAudience = configuration.GetSection("JwtIssuerOptions:Audience").Value,
					RequireExpirationTime = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
					ValidateTokenReplay = true,
					NameClaimType = ClaimTypes.NameIdentifier
				};

				cfg.Events = new()
				{
					OnTokenValidated = static async context =>
					{
						var userId = context.Principal?.Claims.FirstOrDefault(static claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

						if (string.IsNullOrEmpty(userId))
						{
							context.Fail("Invalid token");

							return;
						}

						var isTokenIssuedService = context.HttpContext.RequestServices.GetService<IIsTokenIssuedService>();

						if (await isTokenIssuedService!.IsTokenIssuedAsync(userId))
						{
							return;
						}

						context.Fail("Invalid token");
					}
				};
			})
			.AddTwoFactorUserIdCookie();

		return services;
	}

	internal static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
	{
		var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

		services.Configure<JwtIssuerOptions>(options =>
		{
			options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)]!;
			options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)]!;
			options.SecretKey = jwtAppSettingOptions[nameof(JwtIssuerOptions.SecretKey)]!;

			options.SigningCredentials = new(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettingOptions[nameof(JwtIssuerOptions.SecretKey)]!)),
				SecurityAlgorithms.HmacSha512);

			options.AccessTokenDurationMinutes = int.Parse(jwtAppSettingOptions[nameof(JwtIssuerOptions.AccessTokenDurationMinutes)]!);
			options.RefreshTokenDurationMinutes = int.Parse(jwtAppSettingOptions[nameof(JwtIssuerOptions.RefreshTokenDurationMinutes)]!);
		});

		return services;
	}

	internal static IServiceCollection AddSwagger(this IServiceCollection services) =>
		services.AddSwaggerGen(static swaggerGenOptions =>
		{
			swaggerGenOptions.SwaggerDoc("v1",
				new() { Title = "FurryPets API", Version = "v1" });

			swaggerGenOptions.DescribeAllParametersInCamelCase();

			swaggerGenOptions.AddSecurityDefinition("Bearer", SecurityRequirementsOperationFilter.SecurityScheme);

			swaggerGenOptions.OperationFilter<SecurityRequirementsOperationFilter>();
			swaggerGenOptions.CustomSchemaIds(static type => $"{type.Name}_{Guid.NewGuid()}");

			swaggerGenOptions.SchemaGeneratorOptions.SupportNonNullableReferenceTypes = true;

			swaggerGenOptions.UseAllOfToExtendReferenceSchemas();

			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

			swaggerGenOptions.IncludeXmlComments(xmlPath);
		});
}