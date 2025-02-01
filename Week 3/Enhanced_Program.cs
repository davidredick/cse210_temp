
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Resume Manager!");

        // Create a resume and populate it with data.
        Resume myResume = InitializeResume();

        // Display the resume.
        myResume.Display();
    }

    /// <summary>
    /// Initializes a resume with sample data for demonstration.
    /// </summary>
    /// <returns>A fully populated Resume object.</returns>
    private static Resume InitializeResume()
    {
        // Create a new resume.
        Resume resume = new Resume();
        resume._name = "Allison Rose";

        // Add jobs to the resume using the Job constructor.
        resume._jobs.Add(new Job("Software Engineer", "Microsoft", 2019, 2022));
        resume._jobs.Add(new Job("Manager", "Apple", 2022, 2023));

        return resume;
    }
}
