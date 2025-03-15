using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Handles saving and loading of goals. Saves both active and completed goals.
public static class GoalPersistence
{
    public static void Save(IEnumerable<GoalBase> goals, string filename)
    {
        using StreamWriter writer = new StreamWriter(filename);
        foreach (var goal in goals)
            writer.WriteLine(goal.Serialize());
    }

    public static List<GoalBase> Load(string filename)
    {
        var goals = new List<GoalBase>();
        if (!File.Exists(filename))
        {
            Console.WriteLine("No save file found. Starting fresh.");
            return goals;
        }

        foreach (var line in File.ReadAllLines(filename))
        {
            // Skip empty or whitespace-only lines.
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(':');
            if (parts.Length < 2)
            {
                Console.WriteLine($"Warning: Malformed line skipped: {line}");
                continue;
            }

            string header = parts[0];
            var details = parts[1].Split(',');

            try
            {
                if (header == "SimpleGoal")
                {
                    string name = details[0];
                    bool completed = int.Parse(details[1]) == 1;
                    GoalTerm term = (GoalTerm)Enum.Parse(typeof(GoalTerm), details[4]);
                    SimpleGoal goal = new SimpleGoal(name, term);
                    if (completed)
                        goal.RecordProgress();
                    goals.Add(goal);
                }
                else if (header == "EternalGoal")
                {
                    string name = details[0];
                    int ticks = int.Parse(details[1]);
                    GoalTerm term = (GoalTerm)Enum.Parse(typeof(GoalTerm), details[4]);
                    EternalGoal goal = new EternalGoal(name, term);
                    for (int i = 0; i < ticks; i++)
                        goal.RecordProgress();
                    goals.Add(goal);
                }
                else if (header == "ChecklistGoal")
                {
                    string name = details[0];
                    int ticks = int.Parse(details[1]);
                    int requiredTicks = int.Parse(details[2]);
                    int bonusTicks = int.Parse(details[3]);
                    GoalTerm term = (GoalTerm)Enum.Parse(typeof(GoalTerm), details[6]);
                    ChecklistGoal goal = new ChecklistGoal(name, term, requiredTicks, bonusTicks);
                    for (int i = 0; i < ticks; i++)
                        goal.RecordProgress();
                    goals.Add(goal);
                }
                else if (header == "PercentageGoal")
                {
                    string name = details[0];
                    double percentage = double.Parse(details[1]);
                    GoalTerm term = (GoalTerm)Enum.Parse(typeof(GoalTerm), details[4]);
                    PercentageGoal goal = new PercentageGoal(name, term, percentage);
                    goals.Add(goal);
                }
                else
                {
                    Console.WriteLine($"Warning: Unknown goal type '{header}' found. Skipping line: {line}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Error parsing line: {line}. Exception: {ex.Message}");
            }
        }
        return goals;
    }
}