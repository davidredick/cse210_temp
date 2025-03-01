using System;
using System.Threading.Tasks;

// Extra Extra: List of things done that are extra... export function, broke up each action, read through the rest of the code to see what else I did...  Let me know what you think. :)

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
