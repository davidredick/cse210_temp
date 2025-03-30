using System;
using System.Collections.Generic;

public class Budget
{
    public List<Transaction> Transactions { get; set; }
    public Dictionary<string, decimal> CategoryLimits { get; set; }

    public Budget()
    {
        Transactions = new List<Transaction>();
        CategoryLimits = new Dictionary<string, decimal>();
    }

    public void AddTransaction(Transaction t)
    {
        Transactions.Add(t);
    }

    public void CalculateTotals(out decimal totalIncome, out decimal totalExpenses)
    {
        totalIncome = 0;
        totalExpenses = 0;
        foreach (var transaction in Transactions)
        {
            if (transaction is Income)
                totalIncome += transaction.Amount;
            else if (transaction is Expense)
                totalExpenses += transaction.Amount;
        }
    }
}
