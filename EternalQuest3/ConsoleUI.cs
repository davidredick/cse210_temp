using System;

// A simple console implementation of IUserInterface – classic and reliable.
public class ConsoleUI : IUserInterface
{
    public void Display(string message) => Console.WriteLine(message);
    public string GetInput() => Console.ReadLine() ?? "";
}
