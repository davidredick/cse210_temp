public class Income : Transaction
{
    public Income(string source, decimal amount, DateTime date)
        : base(source, amount, date) { }
}
