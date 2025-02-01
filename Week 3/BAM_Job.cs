
using System;

// The "I worked here, I swear" class.
public class Job
{
    public string _company; // Your "fancy" company name.
    public string _jobTitle; // That title you bragged about on LinkedIn.
    public int _startYear; // The year you said, "I’m totally qualified."
    public int _endYear; // The year you bailed (or got promoted).

    // Spit out the details of this glorious job.
    public void Display()
    {
        Console.WriteLine($"{_jobTitle} ({_company}) {_startYear}-{_endYear}");
    }
}
