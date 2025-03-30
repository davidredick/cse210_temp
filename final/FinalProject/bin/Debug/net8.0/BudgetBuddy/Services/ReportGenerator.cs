using System;
using System.Collections.Generic;
using System.IO;

public class ReportGenerator
{
    public static void GenerateTextReport(List<Transaction> transactions)
    {
        using (StreamWriter writer = new StreamWriter("budgetReport.txt"))
        {
            writer.WriteLine("===== Budget Report =====");
            foreach (var transaction in transactions)
            {
                writer.WriteLine(transaction);
            }
            writer.WriteLine("=========================");
        }
        Console.WriteLine("Report saved as budgetReport.txt");
    }
}
