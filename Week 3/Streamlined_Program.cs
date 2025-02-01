
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Resume Generator! Where dreams of a better GPA come true.");

        // Creating the resume, because Allison needs a job.
        Resume myResume = BuildResume();

        // Show it off.
        myResume.Display();
    }

    // Build a resume with some impressive (fake) data.
    private static Resume BuildResume()
    {
        var resume = new Resume { _name = "Allison Rose" };

        // Add some jobs. Because nobody hires you without 'em.
        resume._jobs.Add(new Job
        {
            _jobTitle = "Software Engineer",
            _company = "Microsoft",
            _startYear = 2019,
            _endYear = 2022
        });

        resume._jobs.Add(new Job
        {
            _jobTitle = "Manager",
            _company = "Apple",
            _startYear = 2022,
            _endYear = 2023
        });

        return resume;
    }
}
