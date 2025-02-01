using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Journal2_0.Models
{
    public class Diary
    {
        private List<Entry> _entries = new List<Entry>();

        public void AddEntry(string prompt, string response, string date)
        {
            _entries.Add(new Entry(date, prompt, response));
        }

        public void DisplayEntries()
        {
            if (_entries.Count == 0)
            {
                Console.WriteLine("No entries yet. Start writing!");
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

        public void SaveToFile(string filename)
        {
            string jsonString = JsonSerializer.Serialize(_entries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filename, jsonString);
        }

        public void LoadFromFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("File not found.");

            string jsonString = File.ReadAllText(filename);
            _entries = JsonSerializer.Deserialize<List<Entry>>(jsonString) ?? new List<Entry>();
        }
    }
}