using System;
using System.IO;
using System.Threading.Tasks;

namespace MindfulnessProgram
{
    // MindfulnessActivity: The wise, abstract sensei of our program.
    // It lays down the foundational behaviors for all our mindful adventures.
    public abstract class MindfulnessActivity
    {
        protected string _activityName;    // The name of the activity – as profound as "Deep Breaths."
        protected string _description;     // A description that might just be as deep as your thoughts.
        protected int _duration;           // Duration in seconds – because good things take time.

        public MindfulnessActivity(string activityName, string description)
        {
            _activityName = activityName;
            _description = description;
        }

        // Initiates the start sequence with a little humor and a lot of zen.
        public virtual async Task DisplayStartMessageAsync()
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the {_activityName} Activity!");
            Console.WriteLine(_description);
            Console.Write("Enter the duration of the activity in seconds: ");
            string? durationInput = Console.ReadLine() ?? string.Empty;
            while (!int.TryParse(durationInput, out _duration) || _duration <= 0)
            {
                Console.WriteLine("Seriously? Enter a valid positive number. We're not magicians here.");
                Console.Write("Enter the duration of the activity in seconds: ");
                durationInput = Console.ReadLine() ?? string.Empty;
            }
            Console.WriteLine("Prepare to begin... (No instant enlightenment, sorry!)");
            await PauseAnimationAsync(3);
        }

        // Concludes the activity with encouraging words (and a dash of sarcasm).
        public virtual async Task DisplayEndMessageAsync()
        {
            Console.WriteLine("\nWell done! You have completed the activity.");
            await PauseAnimationAsync(3);
            Console.WriteLine($"You completed {_activityName} for {_duration} seconds. Bravo!");
            await PauseAnimationAsync(3);
        }

        // A mindful pause to simulate animation and let your anticipation build.
        protected async Task PauseAnimationAsync(int seconds)
        {
            int totalLength = 30;
            for (int i = seconds; i > 0; i--)
            {
                Console.Write($"\rTime remaining: {i,2} sec |");
                int progress = (int)((double)totalLength * i / seconds);
                Console.Write(new string('=', progress).PadRight(totalLength));
                Console.Write("|");
                await Task.Delay(1000);  // Delay: because even the best programs need to catch their breath.
            }
            Console.WriteLine();
        }

        // The abstract method where every derived class shows off its mindful magic.
        public abstract Task RunActivityAsync();
    }
}