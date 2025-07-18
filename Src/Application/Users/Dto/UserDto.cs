using Domain.Entities.Users;

namespace Application.Users.Dto;

public class UserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDto>();
        }
    }    
}