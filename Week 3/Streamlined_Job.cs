
using System;

// The "I worked here, I swear" class.
public class Job
{
    public string _company; // Where you earned your bread.
    public string _jobTitle; // Your very professional-sounding title.
    public int _startYear; // When you started pretending you loved this job.
    public int _endYear; // When you finally escaped.

    // Print the job details. The reader will judge.
    public void Display()
    {
        Console.WriteLine($"{_jobTitle} ({_company}) {_startYear}-{_endYear}");
    }
}
