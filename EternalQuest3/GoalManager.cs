using System.Collections.Generic;
using System.Linq;

// Manages active and completed goals. Completed goals are automatically moved when finished.
public class GoalManager
{
    private readonly List<GoalBase> _activeGoals = new List<GoalBase>();
    private readonly List<GoalBase> _completedGoals = new List<GoalBase>();

    public void AddGoal(GoalBase goal)
    {
        if (_activeGoals.Count < 10)
            _activeGoals.Add(goal);
    }

    public void EraseGoal(int index)
    {
        if (index >= 0 && index < _activeGoals.Count)
            _activeGoals.RemoveAt(index);
    }

    public List<GoalBase> GetGoals() => _activeGoals;
    public List<GoalBase> GetCompletedGoals() => _completedGoals;

    // Scans active goals and moves any completed ones to the completed goals list.
    public void UpdateCompletedGoals()
    {
        for (int i = _activeGoals.Count - 1; i >= 0; i--)
        {
            if (_activeGoals[i].IsComplete())
            {
                _completedGoals.Add(_activeGoals[i]);
                _activeGoals.RemoveAt(i);
            }
        }
    }

    public void LoadGoals(string filename)
    {
        _activeGoals.Clear();
        _activeGoals.AddRange(GoalPersistence.Load(filename));
    }
}
