using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class BudgetManager
{
    private List<Transaction> incomes = new List<Transaction>();
    private List<Transaction> expenses = new List<Transaction>();
    private List<Category> categories = new List<Category>();
    private const string DATA_DIRECTORY = "BudgetData";
    private const string INCOME_FILE = "incomes.json";
    private const string EXPENSE_FILE = "expenses.json";
    private const string CATEGORY_FILE = "categories.json";

    public BudgetManager()
    {
        // Ensure data directory exists
        if (!Directory.Exists(DATA_DIRECTORY))
        {
            Directory.CreateDirectory(DATA_DIRECTORY);
        }
    }

    public void LoadBudget()
    {
        try
        {
            // Load incomes
            if (File.Exists(Path.Combine(DATA_DIRECTORY, INCOME_FILE)))
            {
                string incomesJson = File.ReadAllText(Path.Combine(DATA_DIRECTORY, INCOME_FILE));
                incomes = JsonSerializer.Deserialize<List<Transaction>>(incomesJson) ?? new List<Transaction>();
            }

            // Load expenses
            if (File.Exists(Path.Combine(DATA_DIRECTORY, EXPENSE_FILE)))
            {
                string expensesJson = File.ReadAllText(Path.Combine(DATA_DIRECTORY, EXPENSE_FILE));
                expenses = JsonSerializer.Deserialize<List<Transaction>>(expensesJson) ?? new List<Transaction>();
            }

            // Load categories
            if (File.Exists(Path.Combine(DATA_DIRECTORY, CATEGORY_FILE)))
            {
                string categoriesJson = File.ReadAllText(Path.Combine(DATA_DIRECTORY, CATEGORY_FILE));
                categories = JsonSerializer.Deserialize<List<Category>>(categoriesJson) ?? new List<Category>();
            }
            else
            {
                // Create default categories if none exist
                categories = new List<Category>
                {
                    new Category("Food", 500),
                    new Category("Housing", 1500),
                    new Category("Transportation", 300),
                    new Category("Entertainment", 200),
                    new Category("Other", 400)
                };
            }

            Console.WriteLine("Budget loaded successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading budget: {ex.Message}");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }

    public void SaveBudget()
    {
        try
        {
            // Save incomes
            string incomesJson = JsonSerializer.Serialize(incomes);
            File.WriteAllText(Path.Combine(DATA_DIRECTORY, INCOME_FILE), incomesJson);

            // Save expenses
            string expensesJson = JsonSerializer.Serialize(expenses);
            File.WriteAllText(Path.Combine(DATA_DIRECTORY, EXPENSE_FILE), expensesJson);

            // Save categories
            string categoriesJson = JsonSerializer.Serialize(categories);
            File.WriteAllText(Path.Combine(DATA_DIRECTORY, CATEGORY_FILE), categoriesJson);

            Console.WriteLine("Budget saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving budget: {ex.Message}");
        }
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    public void AddIncome()
    {
        Console.Clear();
        Console.WriteLine("===== Add Income =====");
        
        Console.Write("Description: ");
        string description = Console.ReadLine();
        
        Console.Write("Amount: $");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            Console.WriteLine("Invalid amount. Income not added.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            return;
        }
        
        Transaction income = new Transaction
        {
            Description = description,
            Amount = amount,
            Date = DateTime.Now,
            Category = "Income"
        };
        
        incomes.Add(income);
        Console.WriteLine("Income added successfully!");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    public void AddExpense()
    {
        Console.Clear();
        Console.WriteLine("===== Add Expense =====");
        
        Console.Write("Description: ");
        string description = Console.ReadLine();
        
        Console.Write("Amount: $");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            Console.WriteLine("Invalid amount. Expense not added.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            return;
        }
        
        // Display categories
        Console.WriteLine("\nCategories:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i].Name} (Limit: ${categories[i].Limit})");
        }
        
        Console.Write("Select category (number): ");
        if (!int.TryParse(Console.ReadLine(), out int categoryIndex) || 
            categoryIndex < 1 || 
            categoryIndex > categories.Count)
        {
            Console.WriteLine("Invalid category. Expense not added.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            return;
        }
        
        Transaction expense = new Transaction
        {
            Description = description,
            Amount = amount,
            Date = DateTime.Now,
            Category = categories[categoryIndex - 1].Name
        };
        
        expenses.Add(expense);
        
        // Check if over budget
        decimal totalInCategory = expenses
            .Where(e => e.Category == expense.Category)
            .Sum(e => e.Amount);
            
        Category selectedCategory = categories[categoryIndex - 1];
        if (selectedCategory.CheckSpendingLimit(totalInCategory))
        {
            Console.WriteLine($"Warning: You've exceeded your budget for {selectedCategory.Name}!");
        }
        
        Console.WriteLine("Expense added successfully!");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    public void DisplaySummary()
    {
        Console.Clear();
        Console.WriteLine("===== Budget Summary =====");
        
        decimal totalIncome = incomes.Sum(i => i.Amount);
        decimal totalExpense = expenses.Sum(e => e.Amount);
        decimal balance = totalIncome - totalExpense;
        
        Console.WriteLine($"Total Income: ${totalIncome}");
        Console.WriteLine($"Total Expenses: ${totalExpense}");
        Console.WriteLine($"Balance: ${balance}");
        
        Console.WriteLine("\nExpenses by Category:");
        foreach (var category in categories)
        {
            decimal categoryTotal = expenses
                .Where(e => e.Category == category.Name)
                .Sum(e => e.Amount);
                
            Console.WriteLine($"{category.Name}: ${categoryTotal} (Limit: ${category.Limit})");
            
            if (category.CheckSpendingLimit(categoryTotal))
            {
                Console.WriteLine($"  WARNING: Over budget by ${categoryTotal - category.Limit}!");
            }
        }
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    public void GenerateReport()
    {
        Console.Clear();
        Console.WriteLine("===== Generate Report =====");
        
        Console.WriteLine("1. Monthly Report");
        Console.WriteLine("2. Category Report");
        Console.Write("Choose report type: ");
        
        string choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1":
                GenerateMonthlyReport();
                break;
            case "2":
                GenerateCategoryReport();
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
        
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
    
    private void GenerateMonthlyReport()
    {
        Console.Clear();
        Console.WriteLine("===== Monthly Report =====");
        
        var months = expenses
            .Select(e => new { e.Date.Year, e.Date.Month })
            .Distinct()
            .OrderBy(d => d.Year)
            .ThenBy(d => d.Month);
            
        foreach (var month in months)
        {
            decimal monthlyIncome = incomes
                .Where(i => i.Date.Year == month.Year && i.Date.Month == month.Month)
                .Sum(i => i.Amount);
                
            decimal monthlyExpense = expenses
                .Where(e => e.Date.Year == month.Year && e.Date.Month == month.Month)
                .Sum(e => e.Amount);
                
            Console.WriteLine($"{new DateTime(month.Year, month.Month, 1):MMMM yyyy}:");
            Console.WriteLine($"  Income: ${monthlyIncome}");
            Console.WriteLine($"  Expenses: ${monthlyExpense}");
            Console.WriteLine($"  Balance: ${monthlyIncome - monthlyExpense}");
        }
    }
    
    private void GenerateCategoryReport()
    {
        Console.Clear();
        Console.WriteLine("===== Category Report =====");
        
        foreach (var category in categories)
        {
            var categoryExpenses = expenses
                .Where(e => e.Category == category.Name)
                .OrderBy(e => e.Date);
                
            decimal categoryTotal = categoryExpenses.Sum(e => e.Amount);
            
            Console.WriteLine($"{category.Name} (Total: ${categoryTotal}, Limit: ${category.Limit}):");
            
            foreach (var expense in categoryExpenses)
            {
                Console.WriteLine($"  {expense.Date:MM/dd/yyyy}: {expense.Description} - ${expense.Amount}");
            }
            
            Console.WriteLine();
        }
    }
}