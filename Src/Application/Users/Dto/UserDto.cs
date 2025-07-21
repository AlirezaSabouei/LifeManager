using Domain.Entities.Users;

namespace Application.Users.Dto;

public class UserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? TelegramId { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserDto, User>()
                .ForMember(user => user.Name, opt => opt.MapFrom(userDto => userDto.Name.ToLower()))
                .ReverseMap();
        }
    }    
}