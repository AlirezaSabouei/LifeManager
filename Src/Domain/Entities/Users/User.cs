using LifeManager.Domain.Common;

namespace LifeManager.Domain.Entities.Users;

public class User : BaseAuditableEntity
{
    public required string Name { get; set; }
}