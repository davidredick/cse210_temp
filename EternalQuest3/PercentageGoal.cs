// PercentageGoal: a goal tracked by percentage – perfect for gradual, steady wins.
public class PercentageGoal : GoalBase
{
    private double _percentage;

    public double CurrentPercentage => _percentage;

    public PercentageGoal(string name, GoalTerm term, double initialPercentage)
        : base(name, term, ProgressType.Percentage)
    {
        _percentage = initialPercentage;
    }

    public override void RecordProgress()
    {
        // Not used directly; use LogNewPercentage instead.
    }

    public override bool IsComplete() => _percentage >= 100;

    public override string GetStatus() => $"Percentage: {_percentage}%";

    public override string Serialize()
    {
        // Format: PercentageGoal:Name,Percentage,ConsecutiveDays,TotalDaysWorked,Term,ProgressType
        return $"PercentageGoal:{Name},{_percentage},{ConsecutiveDays},{TotalDaysWorked},{Term},{ProgressType.Percentage}";
    }

    // Logs a new percentage, ensuring progress doesn't go backwards.
    public void LogNewPercentage(double newPercentage)
    {
        if (newPercentage < _percentage)
            return;
        UpdateDayTracking();
        _percentage = newPercentage;
    }
}