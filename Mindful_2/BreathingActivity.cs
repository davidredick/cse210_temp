using System;
using System.Threading.Tasks;

namespace MindfulnessProgram
{
    // BreathingActivity: The class that reminds you to breathe in and out like a zen master.
    public class BreathingActivity : MindfulnessActivity
    {
        private int _breatheInDuration;   // Duration to inhale – deep and steady.
        private int _breatheOutDuration;  // Duration to exhale – let go of your stress.

        public BreathingActivity()
            : base("Breathing", "This activity will help you relax by guiding you through slow, deep breathing. Clear your mind and focus on your breathing.")
        {
        }

        // Extend the start message to include specific breathing instructions.
        public override async Task DisplayStartMessageAsync()
        {
            await base.DisplayStartMessageAsync();
            Console.Write("Enter duration for 'Breathe in' (2-15 seconds): ");
            _breatheInDuration = GetConfigurableTime();
            Console.Write("Enter duration for 'Breathe out' (2-15 seconds): ");
            _breatheOutDuration = GetConfigurableTime();
        }

        // A helper function that ensures you pick a sensible breathing duration.
        private int GetConfigurableTime()
        {
            int value;
            string? input = Console.ReadLine() ?? string.Empty;
            while (!int.TryParse(input, out value) || value < 2 || value > 15)
            {
                Console.WriteLine("Come on, even a sloth can count: enter a number between 2 and 15.");
                Console.Write("Try again: ");
                input = Console.ReadLine() ?? string.Empty;
            }
            return value;
        }

        // Executes the breathing activity with alternating in/out messages.
        public override async Task RunActivityAsync()
        {
            await DisplayStartMessageAsync();
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);
            while (DateTime.Now < endTime)
            {
                Console.WriteLine("\nBreathe in...");
                await PauseAnimationAsync(_breatheInDuration);
                Console.WriteLine("Breathe out...");
                await PauseAnimationAsync(_breatheOutDuration);
            }
            await DisplayEndMessageAsync();
        }
    }
}