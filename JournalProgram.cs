using System;
using System.Collections.Generic;
using System.IO;

namespace DiaryOfADisorganizedSoul
{
    // Main Program Class: Handles menus and coordinates diary misadventures.
    class Program
    {
        static Diary diary = new Diary();
        static List<string> prompts = new List<string>
        {
            "What random thought did you obsess over today?",
            "Which part of your day should be a meme?",
            "Was today a comedy, tragedy, or a really bad sequel? Explain.",
            "Who won the award for 'Most Annoying Person'? Details, please.",
            "If you had a redo button, what moment from today would you hit it for?"
        };

        static void Main(string[] args)
        {
            bool stillJournaling = true;

            while (stillJournaling)
            {
                Console.WriteLine("\nDiary Shenanigans - Menu:");
                Console.WriteLine("1. Here is where you start");
                Console.WriteLine("2. A place to remember and share your best or worst days");
                Console.WriteLine("3. Lock Up Your One-Liners Before They Escape!");
                Console.WriteLine("4. Reload all that you have said");
                Console.WriteLine("5. Run Away - 'No Soup for YOU' (Exit)");
                Console.Write("Choose your adventure (1-5): ");

                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        WriteNewEntry();
                        break;
                    case "2":
                        DisplayDiary();
                        break;
                    case "3":
                        SaveDiaryToFile();
                        break;
                    case "4":
                        LoadDiaryFromFile();
                        break;
                    case "5":
                        Console.WriteLine("Bye! Remember what your Mom told you: 'Remember who you are, and what you stand for'");
                        stillJournaling = false;
                        break;
                    default:
                        Console.WriteLine("Well, that was a swing and a miss. Give it another go 'Champ': Pick 1-5, please!");
                        break;
                }
            }
        }

        // Creates a new diary entry with a random prompt.
        static void WriteNewEntry()
        {
            string prompt = GetRandomPrompt();
            Console.WriteLine($"\nPrompt: {prompt}");
            Console.Write("Word vomit on the page: ");
            string response = Console.ReadLine()?.Trim();
            string date = DateTime.Now.ToShortDateString();

            diary.AddEntry(prompt, response, date);
            Console.WriteLine("Entry Complete: Curious. The implications could be... humorous or regrettable. -Commander Data");
        }

        // Displays all entries in the diary.
        static void DisplayDiary()
        {
            Console.WriteLine("\nYour Diary Entries:");
            diary.DisplayEntries();
        }

        // Saves the diary to a file.
        static void SaveDiaryToFile()
        {
            Console.Write("File to save your masterpiece (e.g., diary.txt): ");
            string filename = Console.ReadLine()?.Trim();
            diary.SaveToFile(filename);
            Console.WriteLine("Saved! You're now immortalized in text!");
        }

        // Loads the diary from a file.
        static void LoadDiaryFromFile()
        {
            Console.Write("File to reload your musings (e.g., diary.txt): ");
            string filename = Console.ReadLine()?.Trim();
            diary.LoadFromFile(filename);
            Console.WriteLine("Loaded! Welcome back to your brain archives.");
        }

        // Retrieves a random prompt from the list.
        static string GetRandomPrompt()
        {
            Random random = new Random();
            return prompts[random.Next(prompts.Count)];
        }
    }

    // Entry Class: Represents a single diary gem.
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

    // Diary Class: Keeps track of all the chaos.
    class Diary
    {
        private readonly List<Entry> entries = new List<Entry>();

        // Adds a new entry to the diary.
        public void AddEntry(string prompt, string response, string date)
        {
            entries.Add(new Entry(date, prompt, response));
        }

        // Displays all diary entries.
        public void DisplayEntries()
        {
            if (entries.Count == 0)
            {
                Console.WriteLine("Nothing here yet. Go live a little, then write about it!");
            }
            else
            {
                foreach (Entry entry in entries)
                {
                    Console.WriteLine(entry);
                    Console.WriteLine(new string('-', 30));
                }
            }
        }

        // Saves all entries to a specified file.
        public void SaveToFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                Console.WriteLine("Invalid filename. Try harder!");
                return;
            }

            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (Entry entry in entries)
                {
                    writer.WriteLine($"{entry.Date}|{entry.Prompt}|{entry.Response}");
                }
            }
        }

        // Loads entries from a specified file.
        public void LoadFromFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                Console.WriteLine("Invalid filename. Let’s not make this a habit.");
                return;
            }

            if (File.Exists(filename))
            {
                entries.Clear();
                string[] lines = File.ReadAllLines(filename);

                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 3)
                    {
                        entries.Add(new Entry(parts[0], parts[1], parts[2]));
                    }
                }
            }
            else
            {
                Console.WriteLine("File not found. Did it run away?");
            }
        }
    }
}
