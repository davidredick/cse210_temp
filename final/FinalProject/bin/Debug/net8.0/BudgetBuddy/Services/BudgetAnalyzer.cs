using System;
using System.Collections.Generic;

public class BudgetAnalyzer
{
    public string AnalyzeBudget(Budget budget)
    {
        decimal totalSpent = 0;
        foreach(var transaction in budget.Transactions)
        {
            if(transaction is Expense)
            {
                totalSpent += transaction.Amount;
            }
        }
        string analysis = $"Total spent on expenses: ${totalSpent}\n";
        foreach(var kvp in budget.CategoryLimits)
        {
            string category = kvp.Key;
            decimal limit = kvp.Value;
            decimal spentInCategory = 0;
            foreach(var transaction in budget.Transactions)
            {
                if(transaction is Expense && transaction.Description.Equals(category, StringComparison.OrdinalIgnoreCase))
                {
                    spentInCategory += transaction.Amount;
                }
            }
            analysis += $"Category {category}: Spent ${spentInCategory} of limit ${limit}\n";
        }
        return analysis;
    }
}
