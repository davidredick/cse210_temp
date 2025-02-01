
using System;
using System.Collections.Generic;

/// <summary>
/// Represents a resume containing a person's name and their job history.
/// </summary>
public class Resume
{
    // The name of the person.
    public string _name;

    // A list to store the jobs associated with the resume.
    public List<Job> _jobs;

    /// <summary>
    /// Constructor to initialize the resume with an empty job list.
    /// </summary>
    public Resume()
    {
        _jobs = new List<Job>();
    }

    /// <summary>
    /// Displays the resume information.
    /// </summary>
    public void Display()
    {
        Console.WriteLine($"Name: {_name}");
        Console.WriteLine("Jobs:");

        // Reuse method to display all jobs.
        DisplayJobs();
    }

    /// <summary>
    /// Displays all jobs in the resume.
    /// </summary>
    private void DisplayJobs()
    {
        foreach (Job job in _jobs)
        {
            job.Display();
        }
    }
}
