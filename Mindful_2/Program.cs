using System;
using System.Threading.Tasks;

namespace MindfulnessProgram
{
    // Program: The entry point where your mindful journey begins.
    class Program
    {
        static async Task Main(string[] args)
        {
            // Because even the best journeys start with a single step (or a single key press).
            MenuManager menuManager = new MenuManager();
            await menuManager.RunAsync();
        }
    }
}