// EternalGoal: a tick-based goal that never completes – like your eternal quest for greatness.
public class EternalGoal : GoalBase
{
    private int _ticks;

    public int Ticks => _ticks;

    public EternalGoal(string name, GoalTerm term) : base(name, term, ProgressType.Ticks)
    {
        _ticks = 0;
    }

    public override void RecordProgress()
    {
        // Only one tick per day – don’t overdo it.
        if (LastUpdateDate.HasValue && LastUpdateDate.Value.Date == System.DateTime.Today)
            return;
        UpdateDayTracking();
        _ticks++;
    }

    public override bool IsComplete() => false; // Eternal goals never complete.

    public override string GetStatus() => $"Ticks: {_ticks} (Eternal)";

    public override string Serialize()
    {
        // Format: EternalGoal:Name,Ticks,ConsecutiveDays,TotalDaysWorked,Term,ProgressType
        return $"EternalGoal:{Name},{_ticks},{ConsecutiveDays},{TotalDaysWorked},{Term},{ProgressType.Ticks}";
    }
}