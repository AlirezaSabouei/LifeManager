using Application.Common.Interfaces;
using Infrustructure.Data;
using Infrustructure.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Infrustructure;

public static class DependencyInjection
{
    public static void AddInfrustructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddDbContext<ApplicationDbContext>(options =>
        //    options.UseSqlServer(
        //        configuration.GetConnectionString("DefaultConnection"),
        //        builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)
        //    ));
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(
                configuration.GetConnectionString("DefaultConnection")
            ));
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<ITelegramBotClient>(provider =>
        {
            return new TelegramBotClient(configuration.GetSection("Telegram").GetValue<string>("BotToken"));
        });
        services.AddScoped<ITelegramBot, TelegramBot>();
    }
}