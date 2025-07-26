using System.Reflection;
using Application.Common.Interfaces;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrustructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().OwnsOne(a => a.WaterIntake);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
}