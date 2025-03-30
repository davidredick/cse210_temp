using System;

public class Category
{
    public string Name { get; set; }
    public decimal Limit { get; set; }

    public Category(string name, decimal limit)
    {
        Name = name;
        Limit = limit;
    }

    public bool CheckSpendingLimit(decimal spent)
    {
        return spent > Limit;
    }
    
    // Parameterless constructor for JSON deserialization
    public Category() { }
}