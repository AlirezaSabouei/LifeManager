using Domain.Common;

namespace Domain.Entities.Users;

public class User : BaseAuditableEntity
{
    public required string Name { get; set; }
}