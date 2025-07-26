using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities.Users;

public class User : BaseAuditableEntity
{
    public required string Name { get; set; } = "Unknown";
    public Int64 TelegramChatId { get; set; }
    public WaterIntake WaterIntake { get; set; }
}