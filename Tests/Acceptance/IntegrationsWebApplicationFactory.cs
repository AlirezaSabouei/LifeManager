using Infrustructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using System.Data.Common;
using Telegram.Bot;
using NSubstitute;
using Telegram.Bot.Types;
using Application.Common.Interfaces;

namespace AcceptanceTests;

public class IntegrationsWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    public ITelegramBot TelegramBotMock { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(IDbContextOptionsConfiguration<ApplicationDbContext>));

            services.Remove(dbContextDescriptor!);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            services.Remove(dbConnectionDescriptor!);

            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<ApplicationDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });

            TelegramBotMock = Substitute.For<ITelegramBot>();
            var telegramBotClient = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(ITelegramBot));
            services.Remove(telegramBotClient!);
            services.AddScoped<ITelegramBot>(a => { return TelegramBotMock; });

        });

        builder.UseEnvironment("Development");
    }
}
