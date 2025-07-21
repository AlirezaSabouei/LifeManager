using Application.Common.Interfaces;
using Infrustructure.Data;
using Infrustructure.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrustructure;

public static class DependencyInjection
{
    public static void AddInfrustructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)
            ));
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IStateManager, StateManager>();
    }
}