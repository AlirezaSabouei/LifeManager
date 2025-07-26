using Domain.Entities.Users;
using Domain.ValueObjects;

namespace Application.Users.Dto;

public class WaterIntakeDto
{
    public int Goal { get; set; }
    public WaterMeasurementUnitDto MeasurementUnit { get; set; }
    public int CurrentIntake { get; set; }
    public int RemainingIntake => Goal - CurrentIntake;
    public bool IsGoalReached => CurrentIntake >= Goal;
    public DateTime LastDay { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserDto, User>()
                .ReverseMap();
            CreateMap<WaterIntakeDto,WaterIntake>()
                .ReverseMap();
        }
    }
}
