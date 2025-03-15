using System;

// Factory for creating new GoalBase instances based on progress type.
public static class GoalFactory
{
    public static GoalBase CreateGoal(ProgressType progressType, string name, GoalTerm term, int requiredTicks = 0, int bonusTicks = 0, double initialPercentage = 0)
    {
        switch (progressType)
        {
            case ProgressType.Ticks:
                // If requiredTicks is 0, treat it as an eternal goal.
                if (requiredTicks == 0)
                    return new EternalGoal(name, term);
                else
                    return new ChecklistGoal(name, term, requiredTicks, bonusTicks);
            case ProgressType.Percentage:
                return new PercentageGoal(name, term, initialPercentage);
            default:
                throw new ArgumentException("Unknown progress type");
        }
    }
}
