
using System;
using System.Collections.Generic;
using System.IO;

// This class is like the "catch-all" for your professional life. No pressure.
public class Resume
{
    public string _name; // Your name. Or someone cooler's name.
    public List<Job> _jobs = new(); // Jobs go here. Obviously.

    // Show the goods (a.k.a. print the resume).
    public void Display()
    {
        Console.WriteLine($"Name: {_name}");
        Console.WriteLine("Jobs:");
        foreach (var job in _jobs)
        {
            job.Display();
        }
    }

    // Save this masterpiece to a file.
    public void SaveToFile(string filePath)
    {
        using StreamWriter writer = new(filePath);
        writer.WriteLine($"Name: {_name}");
        foreach (var job in _jobs)
        {
            writer.WriteLine($"{job._jobTitle}, {job._company}, {job._startYear}-{job._endYear}");
        }
        Console.WriteLine($"Resume saved to {filePath}! BAM!");
    }

    // Bring your resume back from the dead (file).
    public void LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"No file found at {filePath}. Oops.");
            return;
        }

        using StreamReader reader = new(filePath);
        _name = reader.ReadLine()?.Replace("Name: ", "") ?? "Unknown";
        _jobs.Clear();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split(", ");
            if (parts.Length == 3)
            {
                var dateRange = parts[2].Split('-');
                _jobs.Add(new Job
                {
                    _jobTitle = parts[0],
                    _company = parts[1],
                    _startYear = int.Parse(dateRange[0]),
                    _endYear = int.Parse(dateRange[1])
                });
            }
        }
        Console.WriteLine($"Resume loaded from {filePath}! BAM!");
    }
}
