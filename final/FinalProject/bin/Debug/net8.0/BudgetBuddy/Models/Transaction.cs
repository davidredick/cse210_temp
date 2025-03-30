using System;

public abstract class Transaction
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public Transaction(string description, decimal amount, DateTime date)
    {
        Description = description;
        Amount = amount;
        Date = date;
    }

    public override string ToString()
    {
        return $"{Date.ToShortDateString()} - {Description}: ${Amount}";
    }
}
