using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulnessProgram
{
    // MenuManager: The conductor of this mindful orchestra.
    // It handles user input, activity selection, and keeps track of sessions.
    public class MenuManager
    {
        private int breathingCount = 0;   // How many times you've taken a deep breath.
        private int reflectionCount = 0;  // How many reflective moments you've had.
        private int listingCount = 0;     // How many times you've listed your awesomeness.
        private const int MaxSessionsPerActivity = 3;  // Even zen masters need limits.
        private List<(DateTime timestamp, string content)> reflectionSessions = new List<(DateTime, string)>();
        private List<(DateTime timestamp, string content)> listingSessions = new List<(DateTime, string)>();

        // Runs the main menu loop – your gateway to enlightenment.
        public async Task RunAsync()
        {
            bool exitProgram = false;
            while (!exitProgram)
            {
                Console.Clear();
                Console.WriteLine("=== Mindfulness Program Menu ===");
                Console.WriteLine("1. Breathing Activity");
                Console.WriteLine("2. Reflection Activity");
                Console.WriteLine("3. Listing Activity");
                Console.WriteLine("4. Export Sessions");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option (1-5): ");
                string choice = (Console.ReadLine() ?? string.Empty).Trim();
                switch (choice)
                {
                    case "1":
                        if (breathingCount < MaxSessionsPerActivity)
                        {
                            var breathingActivity = new BreathingActivity();
                            await breathingActivity.RunActivityAsync();
                            breathingCount++;
                        }
                        else
                        {
                            Console.WriteLine("You've done enough deep breathing for now. Your lungs might file a complaint.");
                            PauseBeforeMenu();
                        }
                        break;
                    case "2":
                        if (reflectionCount < MaxSessionsPerActivity)
                        {
                            var reflectionActivity = new ReflectionActivity();
                            await reflectionActivity.RunActivityAsync();
                            reflectionSessions.Add((DateTime.Now, reflectionActivity.SessionContent));
                            reflectionCount++;
                        }
                        else
                        {
                            Console.WriteLine("Reflection limit reached. Your brain needs a break from all this introspection.");
                            PauseBeforeMenu();
                        }
                        break;
                    case "3":
                        if (listingCount < MaxSessionsPerActivity)
                        {
                            var listingActivity = new ListingActivity();
                            await listingActivity.RunActivityAsync();
                            listingSessions.Add((DateTime.Now, listingActivity.SessionContent));
                            listingCount++;
                        }
                        else
                        {
                            Console.WriteLine("You've listed enough for now. Even your to-do list deserves a break.");
                            PauseBeforeMenu();
                        }
                        break;
                    case "4":
                        await SessionExporter.HandleExportAsync(reflectionSessions, listingSessions);
                        break;
                    case "5":
                        exitProgram = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please choose a valid option – even indecision is not an option here.");
                        PauseBeforeMenu();
                        break;
                }
            }
            Console.WriteLine("Goodbye! May your mind always be as calm as a college student before finals... almost.");
        }

        // A brief pause before returning to the menu – because even the best need a breather.
        private void PauseBeforeMenu()
        {
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }
    }
}