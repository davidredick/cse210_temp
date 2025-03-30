public class Expense : Transaction
{
    public Expense(string category, decimal amount, DateTime date)
        : base(category, amount, date) { }
}
