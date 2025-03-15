using System;

// Enum for progress type – because sometimes you tick, and sometimes you percent.
public enum ProgressType
{
    Ticks,
    Percentage
}

// Enum for goal term (duration): ShortTerm, MiddleTerm, LongTerm, or Eternal – some dreams last forever.
public enum GoalTerm
{
    ShortTerm,
    MiddleTerm,
    LongTerm,
    Eternal
}

// Abstract base class for all goals – the foundation of your quest.
public abstract class GoalBase
{
    // Private fields for encapsulation (we hide our secrets, like a magician).
    private string _name;
    private GoalTerm _term;
    private int _consecutiveDays;
    private int _totalDaysWorked;
    private DateTime? _lastUpdateDate;
    private ProgressType _progressType;

    // Public properties for controlled access.
    public string Name 
    { 
        get => _name; 
        set => _name = value; 
    }
    public GoalTerm Term 
    { 
        get => _term; 
        set => _term = value; 
    }
    public int ConsecutiveDays => _consecutiveDays;
    public int TotalDaysWorked => _totalDaysWorked;
    public DateTime? LastUpdateDate => _lastUpdateDate;
    public ProgressType ProgressType => _progressType;

    // Constructor that sets the common properties for all goals.
    protected GoalBase(string name, GoalTerm term, ProgressType progressType)
    {
        _name = name;
        _term = term;
        _progressType = progressType;
        _consecutiveDays = 0;
        _totalDaysWorked = 0;
        _lastUpdateDate = null;
    }

    // Updates day tracking details – every day you hustle counts!
    protected void UpdateDayTracking()
    {
        if (_lastUpdateDate.HasValue)
        {
            if (_lastUpdateDate.Value.Date == DateTime.Today.AddDays(-1))
                _consecutiveDays++;
            else if (_lastUpdateDate.Value.Date != DateTime.Today)
                _consecutiveDays = 1;
            _totalDaysWorked++;
        }
        else
        {
            _consecutiveDays = 1;
            _totalDaysWorked = 1;
        }
        _lastUpdateDate = DateTime.Today;
    }

    // Abstract methods to be implemented by derived classes.
    public abstract void RecordProgress();
    public abstract bool IsComplete();
    public abstract string GetStatus();
    public abstract string Serialize();
}