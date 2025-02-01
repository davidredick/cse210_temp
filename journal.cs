using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace JournalApp
{
    // Models a journal entry with all associated metadata.
    class JournalEntry
    {
        public string Prompt { get; set; }
        public string Response { get; set; }
        public string Date { get; set; }
        public string Mood { get; set; }
        public List<string> Tags { get; set; }
        public string Location { get; set; }

        public JournalEntry(string prompt, string response, string mood, List<string> tags, string location)
        {
            Prompt = prompt;
            Response = response;
            Mood = mood;
            Tags = tags;
            Location = location;
            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public override string ToString() =>
            $"Date: {Date}\nPrompt: {Prompt}\nResponse: {Response}\nMood: {Mood}\nTags: {string.Join(", ", Tags)}\nLocation: {Location}\n";
    }

    // Models the journal, which manages entries and handles file operations.
    class Journal
    {
        // Private fields follow _camelCase naming convention.
        private List<JournalEntry> _entries = new List<JournalEntry>();
        private readonly Dictionary<string, List<string>> _prompts = new()
        {
            { "Work", new() { "What was a major accomplishment at work today?", "How did you handle a challenge at work?" } },
            { "Personal", new() { "What was the best part of your personal day?", "Who did you spend quality time with today?" } },
            { "Spiritual", new() { "How did you feel spiritually uplifted today?", "What inspired you spiritually?" } }
        };

        // Adds a new journal entry.
        public void WriteNewEntry()
        {
            string category = GetInput("Choose a category (Work, Personal, Spiritual): ");
            if (!_prompts.ContainsKey(category))
            {
                Console.WriteLine("Invalid category.");
                return;
            }

            string prompt = _prompts[category][new Random().Next(_prompts[category].Count)];
            Console.WriteLine(prompt);
            string response = GetInput("Your response: ");
            string mood = GetInput("Your mood (e.g., Happy, Sad): ");
            string tagsInput = GetInput("Tags (comma-separated): ");
            string location = GetInput("Location: ");
            List<string> tags = tagsInput.Split(',').Select(t => t.Trim()).ToList();

            _entries.Add(new JournalEntry(prompt, response, mood, tags, location));
            Console.WriteLine("Entry saved!");
        }

        // Displays all journal entries.
        public void DisplayJournal()
        {
            if (_entries.Count == 0)
            {
                Console.WriteLine("No entries available.");
                return;
            }
            foreach (var entry in _entries) Console.WriteLine(entry);
        }

        // Saves journal entries to a JSON file.
        public void SaveToJson(string filename)
        {
            try
            {
                string json = JsonSerializer.Serialize(_entries, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filename, json);
                Console.WriteLine($"Journal saved to {filename}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        // Loads journal entries from a JSON file.
        public void LoadFromJson(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine("File not found.");
                    return;
                }
                string json = File.ReadAllText(filename);
                _entries = JsonSerializer.Deserialize<List<JournalEntry>>(json) ?? new List<JournalEntry>();
                Console.WriteLine("Journal loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
            }
        }

        // Helper method for getting user input.
        private static string GetInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }
    }

    // Main program to run the journal application.
    class Program
    {
        static void Main(string[] args)
        {
            // Creativity and exceeding core requirements:
            // - Added metadata fields like Mood, Tags, and Location to enrich journal entries.
            // - Implemented error handling for saving/loading files.
            // - Features a random prompt selection based on categories.

            Journal journal = new();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Write a new entry");
                Console.WriteLine("2. Display all entries");
                Console.WriteLine("3. Save journal to JSON");
                Console.WriteLine("4. Load journal from JSON");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        journal.WriteNewEntry();
                        break;
                    case "2":
                        journal.DisplayJournal();
                        break;
                    case "3":
                        journal.SaveToJson("journal.json");
                        break;
                    case "4":
                        journal.LoadFromJson("journal.json");
                        break;
                    case "5":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }
    }
}
