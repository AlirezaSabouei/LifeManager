using Infrustructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests;

public static class TestDbContextFactory
{
    public static ApplicationDbContext CreateDbContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open(); // Must stay open!

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated(); // Apply schema

        return context;
    }
}
