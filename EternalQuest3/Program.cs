using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Main program for EternalQuest – your ultimate goal-tracking adventure with humor and a dash of sarcasm.
class Program
{
    private const string SaveFilename = "goals.txt";
    private const string BackupFilename = "goals_backup.txt";
    private const string ReadmeFilename = "README.txt";
    private const string QuotesFilename = "lds_quotes.txt";

    // For random quotes – because sometimes you need a funny pick-me-up.
    private static readonly List<int> s_displayedQuoteIndices = new List<int>();
    private static readonly Random s_random = new Random();

    public static void Main(string[] args)
    {
        IUserInterface ui = new ConsoleUI();
        GoalManager manager = new GoalManager();
        GamificationEngine engine = new GamificationEngine();

        // Load saved goals from disk.
        manager.LoadGoals(SaveFilename);
        ui.Display("Goals loaded from disk. Your dreams are safe (for now)!");

        bool exit = false;
        while (!exit)
        {
            DisplayMainMenu(ui);
            string choice = ui.GetInput();
            switch (choice)
            {
                case "1":
                    ManageGoalsMenu(ui, manager, engine);
                    break;
                case "2":
                    exit = true;
                    ui.Display("Exiting application. Goodbye and good luck with your ambitions!");
                    break;
                default:
                    ui.Display("Invalid selection. Please try again (even geniuses make mistakes).");
                    break;
            }
        }
    }

    private static void DisplayMainMenu(IUserInterface ui)
    {
        ui.Display("\n==== MAIN MENU ====");
        ui.Display("1. Manage Goals");
        ui.Display("2. Exit");
        ui.Display("Enter the number corresponding to your choice:");
    }

    // Manage Goals Menu with exactly 9 options:
    // 1) Add Goal, 2) Log Process, 3) Modify Goals, 4) List Search Goals, 5) View Scorecard,
    // 6) Erase Goal, 7) Backup Data, 8) View Instructions, 9) Back to Main Menu.
    private static void ManageGoalsMenu(IUserInterface ui, GoalManager manager, GamificationEngine engine)
    {
        bool back = false;
        while (!back)
        {
            ui.Display("\n==== MANAGE GOALS MENU ====");
            ui.Display("1. Add Goal");
            ui.Display("2. Log Process");
            ui.Display("3. Modify Goals");
            ui.Display("4. List Search Goals");
            ui.Display("5. View Scorecard");
            ui.Display("6. Erase Goal");
            ui.Display("7. Backup Data");
            ui.Display("8. View Instructions");
            ui.Display("9. Back to Main Menu");
            ui.Display("Enter the number corresponding to your choice:");
            string input = ui.GetInput();
            switch (input)
            {
                case "1":
                    AddGoal(ui, manager);
                    AutoSave(manager, ui);
                    break;
                case "2":
                    LogProgress(ui, manager);
                    AutoSave(manager, ui);
                    break;
                case "3":
                    ModifyGoal(ui, manager);
                    AutoSave(manager, ui);
                    break;
                case "4":
                    ListAndFilterGoals(ui, manager);
                    break;
                case "5":
                    DisplayScorecard(ui, manager);
                    break;
                case "6":
                    EraseGoal(ui, manager);
                    AutoSave(manager, ui);
                    break;
                case "7":
                    BackupData(manager, ui);
                    break;
                case "8":
                    DisplayInstructions(ui);
                    break;
                case "9":
                    back = true;
                    break;
                default:
                    ui.Display("Invalid selection. Please try again.");
                    break;
            }
            // Always update completed goals after any operation.
            manager.UpdateCompletedGoals();
        }
    }

    // ----------------------- Goal Management Functions -----------------------

    private static void AddGoal(IUserInterface ui, GoalManager manager)
    {
        // Prompt for goal term (duration).
        ui.Display("Select the goal term (duration):");
        ui.Display("1. Short Term");
        ui.Display("2. Middle Term");
        ui.Display("3. Long Term");
        ui.Display("4. Eternal (tick-based with no completion)");
        string termInput = PromptForValidInput(ui, new[] { "1", "2", "3", "4" }, "Invalid option. Please enter 1, 2, 3, or 4.");
        GoalTerm term = termInput switch
        {
            "1" => GoalTerm.ShortTerm,
            "2" => GoalTerm.MiddleTerm,
            "3" => GoalTerm.LongTerm,
            "4" => GoalTerm.Eternal,
            _ => GoalTerm.ShortTerm
        };

        // Prompt for progress type.
        ui.Display("\nSelect the progress type for your goal:");
        ui.Display("1. Tick-based (enter number of ticks required)");
        ui.Display("2. Percentage-based (enter initial progress percentage)");
        string typeChoice = PromptForValidInput(ui, new[] { "1", "2" }, "Invalid option. Please enter 1 or 2.");
        ProgressType progressType = typeChoice == "1" ? ProgressType.Ticks : ProgressType.Percentage;

        // Ask for goal name.
        ui.Display("Enter goal name:");
        string name = ui.GetInput();

        // Collect progress details.
        int requiredTicks = 0;
        int bonusTicks = 0;
        double initialPercentage = 0;
        if (progressType == ProgressType.Ticks)
        {
            ui.Display("Enter the number of ticks required to complete this goal (enter 0 for an eternal tick-based goal):");
            requiredTicks = GetValidatedInt(ui, "Required ticks:");
            if (requiredTicks > 1)
            {
                ui.Display("Enter bonus ticks awarded upon completion (or 0 if none):");
                bonusTicks = GetValidatedInt(ui, "Bonus ticks:");
            }
        }
        else
        {
            ui.Display("Enter initial progress percentage (0-100):");
            while (!double.TryParse(ui.GetInput(), out initialPercentage) || initialPercentage < 0 || initialPercentage > 100)
            {
                ui.Display("Invalid input. Please enter a number between 0 and 100.");
            }
        }

        GoalBase newGoal = GoalFactory.CreateGoal(progressType, name, term, requiredTicks, bonusTicks, initialPercentage);
        manager.AddGoal(newGoal);
        ui.Display($"Goal '{name}' added successfully! Now go chase it like a caffeinated cheetah!");
    }

    private static void ModifyGoal(IUserInterface ui, GoalManager manager)
    {
        var goals = manager.GetGoals();
        if (!goals.Any())
        {
            ui.Display("No goals to modify. Maybe it's time to create one first?");
            return;
        }
        ui.Display("\n---- Current Goals ----");
        for (int i = 0; i < goals.Count; i++)
            ui.Display($"[{i}] {goals[i].Serialize()}  Status: {goals[i].GetStatus()}");
        ui.Display("Enter the index of the goal you want to modify:");
        int index = GetValidatedInt(ui, "Index:");
        if (index < 0 || index >= goals.Count)
        {
            ui.Display("Invalid index. Returning to menu. (Oops!)");
            return;
        }
        ui.Display("Enter the new name for the goal:");
        string newName = ui.GetInput();
        goals[index].Name = newName;
        ui.Display("Goal modified successfully! A fresh name for a fresh start.");
    }

    private static void EraseGoal(IUserInterface ui, GoalManager manager)
    {
        var goals = manager.GetGoals();
        if (!goals.Any())
        {
            ui.Display("No goals to erase. Your list is as empty as your Herbal Tea cup on Monday morning.");
            return;
        }
        ui.Display("\n---- Current Goals ----");
        for (int i = 0; i < goals.Count; i++)
            ui.Display($"[{i}] {goals[i].Serialize()}  Status: {goals[i].GetStatus()}");
        ui.Display("Enter the index of the goal you want to erase:");
        int idx = GetValidatedInt(ui, "Index:");
        if (idx < 0 || idx >= goals.Count)
        {
            ui.Display("Invalid index. Returning to menu.");
            return;
        }
        manager.EraseGoal(idx);
        ui.Display("Goal erased successfully! Sometimes you need to declutter your dreams.");
    }

    // ----------------------- End Goal Management Functions -----------------------

    #region List And Filter Goals

    private static void ListAndFilterGoals(IUserInterface ui, GoalManager manager)
    {
        bool back = false;
        while (!back)
        {
            ui.Display("\n--- List / Search Goals ---");
            ui.Display("1. List All Goals");
            ui.Display("2. Filter by Term (Short, Middle, Long, Eternal)");
            ui.Display("3. Filter by Completion (Complete/Incomplete)");
            ui.Display("4. Back to Manage Goals Menu");
            ui.Display("Enter your selection:");
            string choice = ui.GetInput();
            List<GoalBase> goals = manager.GetGoals();
            switch (choice)
            {
                case "1":
                    DisplayGoals(ui, goals);
                    break;
                case "2":
                    ui.Display("Enter goal term to filter (Short, Middle, Long, Eternal):");
                    string termFilter = ui.GetInput();
                    var filteredByTerm = goals.Where(g => g.Term.ToString().StartsWith(termFilter, StringComparison.OrdinalIgnoreCase)).ToList();
                    DisplayGoals(ui, filteredByTerm);
                    break;
                case "3":
                    ui.Display("Enter completion status to filter (Complete/Incomplete):");
                    string statusFilter = ui.GetInput().ToLower();
                    var filteredByStatus = goals.Where(g => statusFilter.Contains("complete") ? g.IsComplete() : !g.IsComplete()).ToList();
                    DisplayGoals(ui, filteredByStatus);
                    break;
                case "4":
                    back = true;
                    break;
                default:
                    ui.Display("Invalid selection. Please try again.");
                    break;
            }
        }
    }

    private static void DisplayGoals(IUserInterface ui, List<GoalBase> goals)
    {
        if (!goals.Any())
        {
            ui.Display("No goals to display. Your goal list is as empty as your Herbal Tea cup on Monday morning.");
            return;
        }
        ui.Display("---- Goals ----");
        foreach (var (goal, i) in goals.Select((g, i) => (g, i)))
        {
            string progressDetail = "";
            if (goal.ProgressType == ProgressType.Ticks)
            {
                if (goal is ChecklistGoal cg)
                    progressDetail = cg.RequiredTicks > 0 ? $"{(cg.IsComplete() ? 100 : (int)(((double)cg.Ticks / cg.RequiredTicks) * 100))}%" : "N/A";
                else if (goal is EternalGoal)
                    progressDetail = "N/A";
            }
            else if (goal.ProgressType == ProgressType.Percentage)
            {
                if (goal is PercentageGoal pg)
                    progressDetail = $"{(pg.IsComplete() ? 100 : (int)(pg.CurrentPercentage))}%";
            }
            ui.Display($"[{i}] {goal.Serialize()}  Status: {goal.GetStatus()}  Progress: {progressDetail}");
        }
    }

    #endregion

    #region Log Process (Option #2)

    private static void LogProgress(IUserInterface ui, GoalManager manager)
    {
        List<GoalBase> goals = manager.GetGoals();
        if (!goals.Any())
        {
            ui.Display("No goals available to log progress on. Create one first, champ!");
            return;
        }
        ui.Display("\n---- Goals ----");
        for (int i = 0; i < goals.Count; i++)
            ui.Display($"[{i}] {goals[i].Serialize()}  Status: {goals[i].GetStatus()}");
        ui.Display("Enter the index of the goal you want to work on:");
        int index = GetValidatedInt(ui, "Index:");
        if (index < 0 || index >= goals.Count)
        {
            ui.Display("Invalid index. Returning to menu.");
            return;
        }
        GoalBase goalToLog = goals[index];
        double percentageCompleted = 0;
        int consecutiveDays = goalToLog.ConsecutiveDays;
        int totalDays = goalToLog.TotalDaysWorked;
        if (goalToLog.ProgressType == ProgressType.Ticks)
        {
            if (goalToLog is ChecklistGoal cg)
            {
                cg.RecordProgress();
                if (cg.RequiredTicks > 0)
                    percentageCompleted = (int)(((double)cg.Ticks / cg.RequiredTicks) * 100);
                else
                    percentageCompleted = 0;
                ui.Display("Tick logged for tick-based checklist goal.");
            }
            else if (goalToLog is EternalGoal eg)
            {
                eg.RecordProgress();
                percentageCompleted = 0;
                ui.Display("Tick logged for eternal goal.");
            }
        }
        else if (goalToLog.ProgressType == ProgressType.Percentage)
        {
            if (goalToLog is PercentageGoal pg)
            {
                ui.Display($"Current progress: {pg.CurrentPercentage}%");
                ui.Display("Enter your new progress percentage (must be >= current progress):");
                double newPercent;
                while (!double.TryParse(ui.GetInput(), out newPercent))
                    ui.Display("Invalid input. Please enter a numeric value.");
                if (newPercent < pg.CurrentPercentage)
                {
                    ui.Display("Progress cannot go backwards! Returning to menu.");
                    return;
                }
                pg.LogNewPercentage(newPercent);
                percentageCompleted = pg.CurrentPercentage;
                ui.Display("Progress updated for percentage goal.");
            }
        }
        else
        {
            ui.Display("This goal type does not support progress logging. (Really, what are you trying to do?)");
            return;
        }
        ui.Display("\n*** Reward Summary ***");
        ui.Display($"Percentage Completed: {percentageCompleted}%");
        ui.Display($"Consecutive Days Worked: {consecutiveDays}");
        ui.Display($"Total Days Worked: {totalDays}");
        DisplayRewardQuote(ui);
    }

    #endregion

    #region Scorecard & Instructions

    private static void DisplayScorecard(IUserInterface ui, GoalManager manager)
    {
        ui.Display("\n=== SCORECARD ===");
        ui.Display("Active Goals:");
        List<GoalBase> goals = manager.GetGoals();
        if (!goals.Any())
            ui.Display("No active goals at the moment. Time to set some dreams in motion!");
        else
        {
            foreach (var (goal, i) in goals.Select((g, i) => (g, i)))
            {
                string progressDetail = "";
                if (goal.ProgressType == ProgressType.Ticks)
                {
                    if (goal is ChecklistGoal cg)
                        progressDetail = cg.RequiredTicks > 0 ? $"{(cg.IsComplete() ? 100 : (int)(((double)cg.Ticks / cg.RequiredTicks) * 100))}%" : "N/A";
                    else if (goal is EternalGoal)
                        progressDetail = "N/A";
                }
                else if (goal.ProgressType == ProgressType.Percentage)
                {
                    if (goal is PercentageGoal pg)
                        progressDetail = $"{(pg.IsComplete() ? 100 : (int)(pg.CurrentPercentage))}%";
                }
                ui.Display($"Goal [{i}]: {goal.Serialize()} | Status: {goal.GetStatus()} | Progress: {progressDetail}");
            }
        }
        ui.Display("\n=== COMPLETED GOALS ===");
        var completed = manager.GetCompletedGoals();
        if (!completed.Any())
            ui.Display("No completed goals yet. Keep grinding, superstar!");
        else
        {
            foreach (var (goal, i) in completed.Select((g, i) => (g, i)))
                ui.Display($"Completed Goal [{i}]: {goal.Serialize()} | Final Status: {goal.GetStatus()}");
        }
    }

    private static void DisplayInstructions(IUserInterface ui)
    {
        ui.Display("\n=== INSTRUCTIONS ===");
        if (File.Exists(ReadmeFilename))
            ui.Display(File.ReadAllText(ReadmeFilename));
        else
            ui.Display("README file not found. Please ensure README.txt exists in the application folder.");
    }

    #endregion

    #region Reward System

    private static void DisplayRewardQuote(IUserInterface ui)
    {
        if (!File.Exists(QuotesFilename))
        {
            ui.Display("Quotes file not found. Please ensure lds_quotes.txt exists in the application folder.");
            return;
        }
        string[] quotes = File.ReadAllLines(QuotesFilename);
        if (!quotes.Any())
        {
            ui.Display("No quotes available in lds_quotes.txt.");
            return;
        }
        var availableIndices = Enumerable.Range(0, quotes.Length)
                                         .Where(i => !s_displayedQuoteIndices.Contains(i))
                                         .ToList();
        if (!availableIndices.Any())
        {
            s_displayedQuoteIndices.Clear();
            availableIndices = Enumerable.Range(0, quotes.Length).ToList();
        }
        int selectedIndex = availableIndices[s_random.Next(availableIndices.Count)];
        s_displayedQuoteIndices.Add(selectedIndex);
        ui.Display("\n*** Reward ***");
        ui.Display(quotes[selectedIndex]);
        ui.Display("****************");
    }

    #endregion

    #region Data Persistence

    private static void AutoSave(GoalManager manager, IUserInterface ui)
    {
        try
        {
            // Save both active and completed goals.
            GoalPersistence.Save(manager.GetGoals().Concat(manager.GetCompletedGoals()), SaveFilename);
            ui.Display("Auto-saved goals to disk. Your progress is secure (and so are your dreams)!");
        }
        catch (Exception ex)
        {
            ui.Display($"Error during auto-save: {ex.Message}");
        }
    }

    private static void BackupData(GoalManager manager, IUserInterface ui)
    {
        try
        {
            GoalPersistence.Save(manager.GetGoals().Concat(manager.GetCompletedGoals()), BackupFilename);
            ui.Display($"Data backed up to {BackupFilename}. Backup complete – like an insurance policy for your aspirations!");
        }
        catch (Exception ex)
        {
            ui.Display($"Error during backup: {ex.Message}");
        }
    }

    #endregion

    #region Utility Methods

    private static int GetValidatedInt(IUserInterface ui, string prompt)
    {
        int value;
        while (true)
        {
            ui.Display(prompt);
            if (int.TryParse(ui.GetInput(), out value))
                return value;
            ui.Display("Invalid input. Please enter a numeric value. (Numbers only, please!)");
        }
    }

    private static string PromptForValidInput(IUserInterface ui, IEnumerable<string> validInputs, string errorMsg)
    {
        string input;
        while (!validInputs.Contains(input = ui.GetInput()))
        {
            ui.Display(errorMsg);
        }
        return input;
    }

    #endregion
}