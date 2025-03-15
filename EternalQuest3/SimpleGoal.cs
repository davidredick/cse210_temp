// SimpleGoal: a one-and-done goal that gets checked off like a completed to-do list item.
public class SimpleGoal : GoalBase
{
    private bool _completed;

    public SimpleGoal(string name, GoalTerm term) : base(name, term, ProgressType.Ticks)
    {
        _completed = false;
    }

    public override void RecordProgress()
    {
        // Once done, it's done. No turning back!
        _completed = true;
        UpdateDayTracking();
    }

    public override bool IsComplete() => _completed;

    public override string GetStatus() => _completed ? "[X]" : "[ ]";

    public override string Serialize()
    {
        // Format: SimpleGoal:Name,Completed (1/0),ConsecutiveDays,TotalDaysWorked,Term,ProgressType
        return $"SimpleGoal:{Name},{(_completed ? 1 : 0)},{ConsecutiveDays},{TotalDaysWorked},{Term},{ProgressType.Ticks}";
    }
}