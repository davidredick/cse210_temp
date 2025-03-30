using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class StorageManager
{
    public static void SaveTransactions(List<Transaction> transactions)
    {
        string json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("budgetData.json", json);
    }

    public static List<Transaction> LoadTransactions()
    {
        if (File.Exists("budgetData.json"))
        {
            string json = File.ReadAllText("budgetData.json");
            return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }
        return new List<Transaction>();
    }
}
