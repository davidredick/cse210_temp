using System;
using System.Collections.Generic;
using System.IO;

namespace Journal2_0
{
    // Program Class: Coordinates the Journal (or "Diary") adventures.
    class Program
    {
        // Create a global diary instance
        static Diary _diary = new Diary();

        // List of prompts with a touch of snark and humor.
        static List<string> _prompts = new List<string>
        {
            "Who was the most interesting person you encountered today (even if it was just your mirror)?",
            "What was the best part of your day (or the only part that wasn't a total disaster)?",
            "How did you see the hand of fate (or misfortune) in your life today?",
            "Which moment made you laugh so hard that you nearly snorted?",
            "If you could redo one moment from today, which would it be and why?"
        };

        static void Main(string[] args)
        {
            bool stillJournaling = true;

            while (stillJournaling)
            {
                Console.WriteLine("\n==== Journal2_0: Menu of Misadventures ====");
                Console.WriteLine("1. Write a New Entry");
                Console.WriteLine("2. Display the Journal");
                Console.WriteLine("3. Save the Journal to a File");
                Console.WriteLine("4. Load the Journal from a File");
                Console.WriteLine("5. Exit (Run Away, But Your Thoughts Remain!)");
                Console.Write("Choose your adventure (1-5): ");

                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        WriteNewEntry();
                        break;
                    case "2":
                        DisplayJournal();
                        break;
                    case "3":
                        SaveJournalToFile();
                        break;
                    case "4":
                        LoadJournalFromFile();
                        break;
                    case "5":
                        Console.WriteLine("Farewell, brave soul! Remember: even if the world is chaotic, your journal is your sanctuary.");
                        stillJournaling = false;
                        break;
                    default:
                        Console.WriteLine("Oops! That wasn't a valid option. Please choose between 1 and 5.");
                        break;
                }
            }
        }

        // Creates a new journal entry using a random prompt.
        static void WriteNewEntry()
        {
            string prompt = GetRandomPrompt();
            Console.WriteLine($"\nPrompt: {prompt}");
            Console.Write("Your response (let the word vomit commence): ");
            string response = Console.ReadLine()?.Trim();
            string date = DateTime.Now.ToShortDateString();

            _diary.AddEntry(prompt, response, date);
            Console.WriteLine("Entry added successfully! Who knew your thoughts could be so entertaining?");
        }

        // Displays all the journal entries.
        static void DisplayJournal()
        {
            Console.WriteLine("\n=== Your Journal Entries ===");
            _diary.DisplayEntries();
        }

        // Saves the journal to a file.
        static void SaveJournalToFile()
        {
            Console.Write("Enter the filename to save your journal (e.g., journal.txt): ");
            string filename = Console.ReadLine()?.Trim();
            _diary.SaveToFile(filename);
            Console.WriteLine("Journal saved! Your wisdom is now stored for posterity.");
        }

        // Loads the journal from a file.
        static void LoadJournalFromFile()
        {
            Console.Write("Enter the filename to load your journal from (e.g., journal.txt): ");
            string filename = Console.ReadLine()?.Trim();
            _diary.LoadFromFile(filename);
            Console.WriteLine("Journal loaded! Welcome back to your brain archives.");
        }

        // Retrieves a random prompt from the list.
        static string GetRandomPrompt()
        {
            Random random = new Random();
            return _prompts[random.Next(_prompts.Count)];
        }
    }

    // Entry Class: Represents a single journal entry.
    class Entry
    {
        public string Date { get; set; }
        public string Prompt { get; set; }
        public string Response { get; set; }

        public Entry(string date, string prompt, string response)
        {
            Date = date;
            Prompt = prompt;
            Response = response;
        }

        public override string ToString()
        {
            return $"[{Date}] {Prompt}\n{Response}";
        }
    }

    // Diary Class: Manages the collection of entries.
    class Diary
    {
        private readonly List<Entry> _entries = new List<Entry>();

        // Adds a new entry.
        public void AddEntry(string prompt, string response, string date)
        {
            _entries.Add(new Entry(date, prompt, response));
        }

        // Displays all entries.
        public void DisplayEntries()
        {
            if (_entries.Count == 0)
            {
                Console.WriteLine("Your journal is as empty as your to-do list. Start writing!");
            }
            else
            {
                foreach (Entry entry in _entries)
                {
                    Console.WriteLine(entry);
                    Console.WriteLine(new string('-', 40));
                }
            }
        }

        // Saves all entries to the specified file.
        public void SaveToFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                Console.WriteLine("Invalid filename. Even your file deserves a proper name!");
                return;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    // Use a separator that is unlikely to appear in regular text.
                    foreach (Entry entry in _entries)
                    {
                        writer.WriteLine($"{entry.Date}|{entry.Prompt}|{entry.Response}");
                    }
                }
                Console.WriteLine("Your journal has been saved to the file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the file: {ex.Message}");
            }
        }

        // Loads entries from the specified file, replacing current entries.
        public void LoadFromFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                Console.WriteLine("Invalid filename. Let's try again, shall we?");
                return;
            }

            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found. It seems your journal took a vacation.");
                return;
            }

            try
            {
                _entries.Clear();
                string[] lines = File.ReadAllLines(filename);
                foreach (string line in lines)
                {
                    // Assume the separator '|' is used.
                    string[] parts = line.Split('|');
                    if (parts.Length == 3)
                    {
                        _entries.Add(new Entry(parts[0], parts[1], parts[2]));
                    }
                }
                Console.WriteLine("Your journal has been reloaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the file: {ex.Message}");
            }
        }
    }
}
