using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MindfulnessProgram
{
    // ListingActivity: Helps you list out all the awesome things in your life – one item at a time.
    public class ListingActivity : MindfulnessActivity
    {
        private List<string> _listEntries = new List<string>();  // Your list of greatness.
        private readonly List<string> _prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };

        // Aggregated session content: your final list, for posterity.
        private string _sessionContent = string.Empty;
        public string SessionContent => _sessionContent;

        public ListingActivity()
            : base("Listing", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
        {
        }

        // Runs the listing activity – time to see just how many awesome things you can list.
        public override async Task RunActivityAsync()
        {
            await DisplayStartMessageAsync();
            Random rand = new Random();
            string chosenPrompt = _prompts[rand.Next(_prompts.Count)];
            Console.WriteLine($"\nList prompt: {chosenPrompt}");
            Console.WriteLine("Get ready...");
            await PauseAnimationAsync(5);
            Console.WriteLine("\nStart listing your items. Press Enter after each one.");
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);
            while (DateTime.Now < endTime)
            {
                if (Console.KeyAvailable)
                {
                    string entry = Console.ReadLine() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(entry))
                    {
                        _listEntries.Add(entry.Trim());
                    }
                }
                else
                {
                    await Task.Delay(200);
                }
            }
            Console.WriteLine("\nYou listed the following items:");
            for (int i = 0; i < _listEntries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_listEntries[i]}");
            }
            await DisplayEndMessageAsync();
            SaveListingSession();
        }

        // Saves your list to a text file – because your ideas deserve a backup.
        private void SaveListingSession()
        {
            _sessionContent = string.Join(Environment.NewLine, _listEntries);
            string filename = $"List_{DateTime.Now:MM_dd_yyyy__HHmmss}.txt";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                for (int i = 0; i < _listEntries.Count; i++)
                {
                    writer.WriteLine($"{i + 1}. {_listEntries[i]}");
                }
            }
            Console.WriteLine($"\nListing session saved to file: {filename}");
        }
    }
}