using FurryPets.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FurryPets.Di.DiManagers;

public static class DataDi
{
    public static IServiceCollection AddData(this IServiceCollection services, string connectionString) =>
        services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    sqlServerDbContextOptionsBuilder =>
                        sqlServerDbContextOptionsBuilder.MigrationsAssembly("FurryPets.Infrastructure"));
            })
            .AddScoped<DataContext>();
}