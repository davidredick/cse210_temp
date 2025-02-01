using System;
using System.Collections.Generic;
using Journal2_0.Models;

namespace Journal2_0
{
    class Program
    {
        static Diary _diary = new Diary();
        static Random _random = new Random();
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
                Console.WriteLine("5. Exit");
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
                        Console.WriteLine("Farewell, brave soul!");
                        stillJournaling = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose between 1 and 5.");
                        break;
                }
            }
        }

        static void WriteNewEntry()
        {
            string prompt = GetRandomPrompt();
            Console.WriteLine($"\nPrompt: {prompt}");
            Console.Write("Your response: ");
            string response = Console.ReadLine()?.Trim();
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            _diary.AddEntry(prompt, response, dateTime);
            Console.WriteLine("Entry added successfully!");
        }

        static void DisplayJournal()
        {
            Console.WriteLine("\n=== Your Journal Entries ===");
            _diary.DisplayEntries();
        }

        static void SaveJournalToFile()
        {
            Console.Write("Enter the filename to save your journal (e.g., journal.json): ");
            string filename = Console.ReadLine()?.Trim();
            try
            {
                _diary.SaveToFile(filename);
                Console.WriteLine("Journal saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving: {ex.Message}");
            }
        }

        static void LoadJournalFromFile()
        {
            Console.Write("Enter the filename to load your journal from (e.g., journal.json): ");
            string filename = Console.ReadLine()?.Trim();
            try
            {
                _diary.LoadFromFile(filename);
                Console.WriteLine("Journal loaded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading: {ex.Message}");
            }
        }

        static string GetRandomPrompt()
        {
            return _prompts[_random.Next(_prompts.Count)];
        }
    }
}