using Application.Users.Dto;
using Domain.Entities.Users;

namespace Application.Users.Dto;

public class UserDto
{
    public long Id { get; set; }
    public required string Name { get; set; } = "Unknown";
    public Int64 TelegramChatId { get; set; }
    public WaterIntakeDto WaterIntake { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserDto, User>()
                .ReverseMap();
        }
    }
}