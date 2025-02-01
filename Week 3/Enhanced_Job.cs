
using System;

/// <summary>
/// Represents a job with details like title, company, and employment duration.
/// </summary>
public class Job
{
    // The company where the job was held.
    public string _company;

    // The job title.
    public string _jobTitle;

    // The year the job started.
    public int _startYear;

    // The year the job ended.
    public int _endYear;

    /// <summary>
    /// Constructor to initialize a job with given details.
    /// </summary>
    public Job(string jobTitle, string company, int startYear, int endYear)
    {
        _jobTitle = jobTitle;
        _company = company;
        _startYear = startYear;
        _endYear = endYear;
    }

    /// <summary>
    /// Displays the job details in a formatted manner.
    /// </summary>
    public void Display()
    {
        Console.WriteLine($"{_jobTitle} ({_company}) {_startYear}-{_endYear}");
    }
}
