using Domain.Enums;

namespace Domain.ValueObjects;

public class WaterIntake
{
    public WaterIntake()
    {
        if (LastDay == default)
        {
            LastDay = DateTime.UtcNow;
        }
    }

    public int Goal { get; set; }
    public WaterMeasurementUnit MeasurementUnit { get; set; } = WaterMeasurementUnit.Glass;
    public int CurrentIntake { get; set; } = 0;
    public int RemainingIntake => Goal - CurrentIntake;
    public bool IsGoalReached => CurrentIntake >= Goal;
    public DateTime LastDay { get; set; }

    public void Reset()
    {
        CurrentIntake = 0;
        LastDay = DateTime.Now;
    }

    public void AddIntake(int amount)
    {
        if (LastDay.Day != DateTime.Now.Day)
        {
            Reset();
        }
        CurrentIntake += amount;
    }
}
