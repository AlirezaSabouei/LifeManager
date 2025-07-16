using LifeManager.Domain.Entities.Users;

namespace LifeManager.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
}