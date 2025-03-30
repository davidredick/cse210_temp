using System;

class Program
{
    static void Main()
    {
        BudgetManager budgetManager = new BudgetManager();
        budgetManager.LoadBudget();

        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("===== BudgetBuddy =====");
            Console.WriteLine("1. Add Income");
            Console.WriteLine("2. Add Expense");
            Console.WriteLine("3. View Budget Summary");
            Console.WriteLine("4. Generate Report");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    budgetManager.AddIncome();
                    break;
                case "2":
                    budgetManager.AddExpense();
                    break;
                case "3":
                    budgetManager.DisplaySummary();
                    break;
                case "4":
                    budgetManager.GenerateReport();
                    break;
                case "5":
                    budgetManager.SaveBudget();
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }
}
