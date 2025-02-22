using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MindfulnessProgram
{
    // Base class that all our mindful activities inherit from.
    // Think of this as the wise sensei who shares all the cool secrets.
    public abstract class MindfulnessActivity
    {
        protected string _activityName;
        protected string _description;
        protected int _duration; // Duration in seconds for the whole activity

        public MindfulnessActivity(string activityName, string description)
        {
            _activityName = activityName;
            _description = description;
        }

        public virtual void DisplayStartMessage()
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the {_activityName} Activity!");
            Console.WriteLine(_description);
            Console.Write("Enter the duration of the activity in seconds: ");
            while (!int.TryParse(Console.ReadLine(), out _duration) || _duration <= 0)
            {
                Console.WriteLine("Seriously? Enter a valid positive number. We're not magicians here.");
                Console.Write("Enter the duration of the activity in seconds: ");
            }
            Console.WriteLine("Prepare to begin...");
            PauseAnimation(3);
        }

        public virtual void DisplayEndMessage()
        {
            Console.WriteLine("\nWell done! You have completed the activity.");
            PauseAnimation(3);
            Console.WriteLine($"You completed {_activityName} for {_duration} seconds. Bravo!");
            PauseAnimation(3);
        }

        protected void PauseAnimation(int seconds)
        {
            int totalLength = 30;
            for (int i = seconds; i > 0; i--)
            {
                Console.Write($"\rTime remaining: {i,2} sec |");
                int progress = (int)((double)totalLength * i / seconds);
                Console.Write(new string('=', progress).PadRight(totalLength));
                Console.Write("|");
                Thread.Sleep(1000);
            }
            Console.WriteLine();
        }

        public abstract void RunActivity();
    }

    // BreathingActivity: Helps you breathe like a zen master (or at least try to).
    public class BreathingActivity : MindfulnessActivity
    {
        private int _breatheInDuration;
        private int _breatheOutDuration;

        public BreathingActivity()
            : base("Breathing", "This activity will help you relax by guiding you through slow, deep breathing. Clear your mind and focus on your breathing.")
        {
        }

        public override void DisplayStartMessage()
        {
            base.DisplayStartMessage();
            Console.Write("Enter duration for 'Breathe in' (2-15 seconds): ");
            _breatheInDuration = GetConfigurableTime();
            Console.Write("Enter duration for 'Breathe out' (2-15 seconds): ");
            _breatheOutDuration = GetConfigurableTime();
        }

        private int GetConfigurableTime()
        {
            int value;
            while (!int.TryParse(Console.ReadLine(), out value) || value < 2 || value > 15)
            {
                Console.WriteLine("Come on, even a sloth can count: enter a number between 2 and 15.");
                Console.Write("Try again: ");
            }
            return value;
        }

        public override void RunActivity()
        {
            DisplayStartMessage();
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);
            while (DateTime.Now < endTime)
            {
                Console.WriteLine("\nBreathe in...");
                PauseAnimation(_breatheInDuration);
                Console.WriteLine("Breathe out...");
                PauseAnimation(_breatheOutDuration);
            }
            DisplayEndMessage();
        }
    }

    // ReflectionActivity: Get deep in your thoughts without falling asleep.
    public class ReflectionActivity : MindfulnessActivity
    {
        private List<string> _journalEntries = new List<string>();
        private bool _journalingMode;

        private readonly List<string> _allQuestions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        };

        private List<string> _shuffledQuestions = new List<string>();
        private int _questionIndex = 0;

        private readonly List<string> _prompts = new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };

        // Aggregated session content
        private string _sessionContent = string.Empty;
        public string SessionContent => _sessionContent;

        public ReflectionActivity()
            : base("Reflection", "This activity will help you reflect on times in your life when you have shown strength and resilience. Recognize your power and learn from your experiences.")
        {
            ReshuffleQuestions();
        }

        public override void DisplayStartMessage()
        {
            base.DisplayStartMessage();
            Console.Write("Would you like to use this as a journaling exercise? (y/n): ");
            string input = Console.ReadLine().Trim().ToLower();
            _journalingMode = (input == "y" || input == "yes");
        }

        private void ReshuffleQuestions()
        {
            _shuffledQuestions = new List<string>(_allQuestions);
            Random rand = new Random();
            for (int i = _shuffledQuestions.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                string temp = _shuffledQuestions[i];
                _shuffledQuestions[i] = _shuffledQuestions[j];
                _shuffledQuestions[j] = temp;
            }
            _questionIndex = 0;
        }

        private string GetNextQuestion()
        {
            if (_questionIndex >= _shuffledQuestions.Count)
            {
                ReshuffleQuestions();
            }
            return _shuffledQuestions[_questionIndex++];
        }

        public override void RunActivity()
        {
            DisplayStartMessage();
            Random rand = new Random();
            string chosenPrompt = _prompts[rand.Next(_prompts.Count)];
            Console.WriteLine($"\nReflect on the following prompt:\n--- {chosenPrompt} ---");
            PauseAnimation(5);

            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);
            while (DateTime.Now < endTime)
            {
                string question = GetNextQuestion();
                Console.WriteLine($"\n{question}");
                if (_journalingMode)
                {
                    Console.Write("Your response (type and press Enter): ");
                    string response = Console.ReadLine();
                    _journalEntries.Add($"{DateTime.Now}: Q: {question} A: {response}");
                }
                else
                {
                    PauseAnimation(5);
                }
            }
            DisplayEndMessage();
            SaveReflectionSession();
        }

        private void SaveReflectionSession()
        {
            _sessionContent = string.Join(Environment.NewLine, _journalEntries);
            string filename = $"Reflect_{DateTime.Now:MM_dd_yyyy__HHmmss}.txt";
            File.WriteAllLines(filename, _journalEntries);
            Console.WriteLine($"\nReflection session saved to file: {filename}");
        }
    }

    // ListingActivity: List out all the things that make life awesome (or just your favorite snacks).
    public class ListingActivity : MindfulnessActivity
    {
        private List<string> _listEntries = new List<string>();

        private readonly List<string> _prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };

        // Aggregated session content
        private string _sessionContent = string.Empty;
        public string SessionContent => _sessionContent;

        public ListingActivity()
            : base("Listing", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
        {
        }

        public override void RunActivity()
        {
            DisplayStartMessage();
            Random rand = new Random();
            string chosenPrompt = _prompts[rand.Next(_prompts.Count)];
            Console.WriteLine($"\nList prompt: {chosenPrompt}");
            Console.WriteLine("Get ready...");
            PauseAnimation(5);

            Console.WriteLine("\nStart listing your items. Press Enter after each one.");
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);
            while (DateTime.Now < endTime)
            {
                if (Console.KeyAvailable)
                {
                    string entry = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(entry))
                    {
                        _listEntries.Add(entry.Trim());
                    }
                }
                else
                {
                    Thread.Sleep(200);
                }
            }

            Console.WriteLine("\nYou listed the following items:");
            for (int i = 0; i < _listEntries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_listEntries[i]}");
            }
            DisplayEndMessage();
            SaveListingSession();
        }

        private void SaveListingSession()
        {
            _sessionContent = string.Join(Environment.NewLine, _listEntries);
            string filename = $"List_{DateTime.Now:MM_dd_yyyy__HHmmss}.txt";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                for (int i = 0; i < _listEntries.Count; i++)
                {
                    writer.WriteLine($"{i + 1}. {_listEntries[i]}");
                }
            }
            Console.WriteLine($"\nListing session saved to file: {filename}");
        }
    }

    // Main program class: the command center of our mindful adventure.
    class Program
    {
        private static int breathingCount = 0;
        private static int reflectionCount = 0;
        private static int listingCount = 0;
        private const int MaxSessionsPerActivity = 3;

        // Local collections to aggregate session data for export.
        private static List<(DateTime timestamp, string content)> reflectionSessions = new List<(DateTime, string)>();
        private static List<(DateTime timestamp, string content)> listingSessions = new List<(DateTime, string)>();

        static void Main(string[] args)
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

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (breathingCount < MaxSessionsPerActivity)
                        {
                            BreathingActivity breathingActivity = new BreathingActivity();
                            breathingActivity.RunActivity();
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
                            ReflectionActivity reflectionActivity = new ReflectionActivity();
                            reflectionActivity.RunActivity();
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
                            ListingActivity listingActivity = new ListingActivity();
                            listingActivity.RunActivity();
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
                        HandleExport();
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

        private static void PauseBeforeMenu()
        {
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        // Modified export functionality: default location is the local desktop.
        private static void HandleExport()
        {
            Console.Clear();
            Console.WriteLine("=== Export Sessions ===");
            Console.WriteLine("Do you really want to export? (Because exporting is serious business) (y/n): ");
            string exportChoice = Console.ReadLine().Trim().ToLower();
            if (exportChoice != "y" && exportChoice != "yes")
            {
                Console.WriteLine("Export aborted. Your sessions remain safely in limbo.");
                PauseBeforeMenu();
                return;
            }

            // Default export location: Desktop.
            string defaultExportDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Console.WriteLine($"\nThe default export location is your Desktop: {defaultExportDir}");
            Console.Write("Do you want to change it? (y/n): ");
            string changeDir = Console.ReadLine().Trim().ToLower();
            string exportDir = defaultExportDir;
            if (changeDir == "y" || changeDir == "yes")
            {
                Console.Write("Enter the full directory path to export to: ");
                exportDir = Console.ReadLine();
                if (!Directory.Exists(exportDir))
                {
                    Console.WriteLine("Directory does not exist. Creating it because we believe in second chances.");
                    Directory.CreateDirectory(exportDir);
                }
            }

            // Ask for the output file name.
            Console.Write("Enter the output file name (without extension): ");
            string outputFileName = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(outputFileName))
            {
                outputFileName = "ExportedSessions";
            }
            outputFileName += ".txt";

            Console.WriteLine("\nWhat would you like to export?");
            Console.WriteLine("1. Reflection Sessions");
            Console.WriteLine("2. Listing Sessions");
            Console.Write("Choose (1-2): ");
            string typeChoice = Console.ReadLine();

            if (typeChoice == "1")
            {
                // Merge all reflection sessions into one document.
                ExportMergedSessions(reflectionSessions, outputFileName, exportDir, "Reflection");
            }
            else if (typeChoice == "2")
            {
                // Merge all listing sessions into one document.
                ExportMergedSessions(listingSessions, outputFileName, exportDir, "Listing");
            }
            else
            {
                Console.WriteLine("Invalid export option selected. Export canceled.");
            }
            PauseBeforeMenu();
        }

        // Aggregates all session entries and merges them into one document.
        private static void ExportMergedSessions(List<(DateTime timestamp, string content)> sessions, string fileName, string directory, string sessionType)
        {
            if (sessions.Count == 0)
            {
                Console.WriteLine($"No {sessionType} sessions to export. Maybe next time you'll be more productive?");
                return;
            }

            sessions.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
            string fullPath = Path.Combine(directory, fileName);
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine($"=== Merged {sessionType} Sessions Export ===");
                writer.WriteLine();
                foreach (var session in sessions)
                {
                    writer.WriteLine($"--- {session.timestamp:MM/dd/yyyy HH:mm:ss} ---");
                    writer.WriteLine(session.content);
                    writer.WriteLine(new string('-', 50));
                    writer.WriteLine();
                }
            }
            Console.WriteLine($"Export successful! Merged {sessionType} sessions have been exported to: {fullPath}");
        }
    }
}
