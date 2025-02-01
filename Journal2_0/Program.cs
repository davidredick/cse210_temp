using System;
using System.Collections.Generic;
using Journal2_0.Models;

namespace Journal2_0
{
    /*
        Journal2_0 Program
        -------------------
        This program helps users keep a journal by providing a daily writing prompt,
        saving each entry with the prompt, response, and the date/time. It uses JSON
        serialization for robust saving/loading and includes creative, playful messages.
    */
    class Program
    {
        // Global diary instance.
        static Diary _diary = new Diary();

        // A single, static Random instance for prompt generation.
        static Random _random = new Random();

        // List of writing prompts.
        static List<string> _prompts = new List<string>
        {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?",
            "What unexpected event made you smile today?",
            "What lesson did you learn today?"
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
            // Enhanced date/time handling: include both date and time.
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            _diary.AddEntry(prompt, response, dateTime);
            Console.WriteLine("Entry added successfully! Who knew your thoughts could be so entertaining?");
        }

        // Displays all the journal entries.
        static void DisplayJournal()
        {
            Console.WriteLine("\n=== Your Journal Entries ===");
            _diary.DisplayEntries();
        }

        // Saves the journal to a file using JSON serialization.
        static void SaveJournalToFile()
        {
            Console.Write("Enter the filename to save your journal (e.g., journal.json): ");
            string filename = Console.ReadLine()?.Trim();
            try
            {
                _diary.SaveToFile(filename);
                Console.WriteLine("Journal saved! Your wisdom is now stored for posterity.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Whoa, hold on! An error occurred while saving: {ex.Message}");
            }
        }

        // Loads the journal from a file using JSON deserialization.
        static void LoadJournalFromFile()
        {
            Console.Write("Enter the filename to load your journal from (e.g., journal.json): ");
            string filename = Console.ReadLine()?.Trim();
            try
            {
                _diary.LoadFromFile(filename);
                Console.WriteLine("Journal loaded! Welcome back to your brain archives.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Yikes! An error occurred while loading: {ex.Message}");
            }
        }

        // Retrieves a random prompt from the list.
        static string GetRandomPrompt()
        {
            return _prompts[_random.Next(_prompts.Count)];
        }
    }
}
