using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Journal2_0.Models
{
    public class Diary
    {
        private List<Entry> _entries = new List<Entry>();

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

        // Saves all entries to the specified file using JSON.
        public void SaveToFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Invalid filename. Even your file deserves a proper name!");

            try
            {
                string jsonString = JsonSerializer.Serialize(_entries, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filename, jsonString);
            }
            catch (Exception)
            {
                // Rethrow to be handled by the calling method.
                throw;
            }
        }

        // Loads entries from the specified file, replacing current entries.
        public void LoadFromFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Invalid filename. Let's try again, shall we?");

            if (!File.Exists(filename))
                throw new FileNotFoundException("File not found. It seems your journal took a vacation.");

            try
            {
                string jsonString = File.ReadAllText(filename);
                List<Entry> loadedEntries = JsonSerializer.Deserialize<List<Entry>>(jsonString);
                if (loadedEntries != null)
                {
                    _entries = loadedEntries;
                }
                else
                {
                    Console.WriteLine("Hmm, the file was empty or in an unexpected format. No entries loaded.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
