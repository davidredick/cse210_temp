using System;

public class User
{
    public string Name { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal SavingsGoal { get; set; }

    public User(string name, decimal totalIncome, decimal savingsGoal)
    {
        Name = name;
        TotalIncome = totalIncome;
        SavingsGoal = savingsGoal;
    }

    public void SetBudget(decimal income, decimal savingsGoal)
    {
        TotalIncome = income;
        SavingsGoal = savingsGoal;
    }

    public void ViewBudgetSummary(Budget budget)
    {
        Console.WriteLine($"User: {Name}");
        Console.WriteLine($"Total Income: ${TotalIncome}");
        Console.WriteLine($"Savings Goal: ${SavingsGoal}");
        Console.WriteLine($"Budget Analysis:\n{new BudgetAnalyzer().AnalyzeBudget(budget)}");
    }
}
