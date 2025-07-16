using System.Reflection;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace LifeManager.Infrustructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
`
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}