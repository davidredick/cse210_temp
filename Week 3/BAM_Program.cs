
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the BAM Resume Builder! Let’s make you look good.");

        Resume myResume = BuildResume();

        while (true)
        {
            Console.WriteLine("\nWhat do you want to do?");
            Console.WriteLine("1. Show Resume\n2. Add Job\n3. Save to File\n4. Load from File\n5. Quit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    myResume.Display();
                    break;
                case "2":
                    AddJobToResume(myResume);
                    break;
                case "3":
                    Console.Write("Enter file name to save: ");
                    string saveFile = Console.ReadLine();
                    myResume.SaveToFile(saveFile);
                    break;
                case "4":
                    Console.Write("Enter file name to load: ");
                    string loadFile = Console.ReadLine();
                    myResume.LoadFromFile(loadFile);
                    break;
                case "5":
                    Console.WriteLine("Bye! BAM!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    // Build a resume with some starter data.
    private static Resume BuildResume()
    {
        var resume = new Resume { _name = "Allison Rose" };
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

    // Add a job interactively to the resume.
    private static void AddJobToResume(Resume resume)
    {
        Console.WriteLine("Let’s add a new job to your resume.");
        Console.Write("Enter job title: ");
        string jobTitle = Console.ReadLine();
        Console.Write("Enter company name: ");
        string company = Console.ReadLine();
        Console.Write("Enter start year: ");
        int startYear = int.Parse(Console.ReadLine());
        Console.Write("Enter end year: ");
        int endYear = int.Parse(Console.ReadLine());

        resume._jobs.Add(new Job
        {
            _jobTitle = jobTitle,
            _company = company,
            _startYear = startYear,
            _endYear = endYear
        });

        Console.WriteLine($"Added "{jobTitle}" at {company}! BAM!");
    }
}
