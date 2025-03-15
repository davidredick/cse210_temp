// ChecklistGoal: a tick-based goal requiring a set number of ticks, with bonus ticks as an extra high-five when done.
public class ChecklistGoal : GoalBase
{
    private int _ticks;
    private int _requiredTicks;
    private int _bonusTicks;

    public int Ticks => _ticks;
    public int RequiredTicks => _requiredTicks;
    public int BonusTicks => _bonusTicks;

    public ChecklistGoal(string name, GoalTerm term, int requiredTicks, int bonusTicks)
        : base(name, term, ProgressType.Ticks)
    {
        _ticks = 0;
        _requiredTicks = requiredTicks;
        _bonusTicks = bonusTicks;
    }

    public override void RecordProgress()
    {
        // Only log one tick per day.
        if (LastUpdateDate.HasValue && LastUpdateDate.Value.Date == System.DateTime.Today)
            return;
        UpdateDayTracking();
        _ticks++;
        // When target is met, award bonus ticks (only once).
        if (_ticks >= _requiredTicks && _bonusTicks > 0)
        {
            _ticks += _bonusTicks;
            _bonusTicks = 0;
        }
    }

    public override bool IsComplete() => _requiredTicks > 0 && _ticks >= _requiredTicks;

    public override string GetStatus() => $"Ticks: {_ticks}/{_requiredTicks}";

    public override string Serialize()
    {
        // Format: ChecklistGoal:Name,Ticks,RequiredTicks,BonusTicks,ConsecutiveDays,TotalDaysWorked,Term,ProgressType
        return $"ChecklistGoal:{Name},{_ticks},{_requiredTicks},{_bonusTicks},{ConsecutiveDays},{TotalDaysWorked},{Term},{ProgressType.Ticks}";
    }
}