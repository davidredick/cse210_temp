using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MindfulnessProgram
{
    // ReflectionActivity: Encourages you to reflect deeply, without nodding off.
    public class ReflectionActivity : MindfulnessActivity
    {
        private List<string> _journalEntries = new List<string>();  // Captures your thoughtful responses.
        private bool _journalingMode;  // True if you’re in journaling mode (because your thoughts deserve to be recorded).
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
        private List<string> _shuffledQuestions = new List<string>();  // Questions shuffled for unpredictable introspection.
        private int _questionIndex = 0;  // Tracks which question you're pondering.
        private readonly List<string> _prompts = new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };

        // The aggregated session content is your intellectual masterpiece.
        private string _sessionContent = string.Empty;
        public string SessionContent => _sessionContent;

        public ReflectionActivity()
            : base("Reflection", "This activity will help you reflect on times in your life when you have shown strength and resilience. Recognize your power and learn from your experiences.")
        {
            ReshuffleQuestions();
        }

        // Overrides the start message to ask if you want to journal your inner wisdom.
        public override async Task DisplayStartMessageAsync()
        {
            await base.DisplayStartMessageAsync();
            Console.Write("Would you like to use this as a journaling exercise? (y/n): ");
            string input = (Console.ReadLine() ?? string.Empty).Trim().ToLower();
            _journalingMode = (input == "y" || input == "yes");
        }

        // Reshuffles questions to ensure no two sessions feel exactly the same.
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

        // Gets the next question, reshuffling if needed.
        private string GetNextQuestion()
        {
            if (_questionIndex >= _shuffledQuestions.Count)
            {
                ReshuffleQuestions();
            }
            return _shuffledQuestions[_questionIndex++];
        }

        // Runs the reflection activity until your inner monologue is complete.
        public override async Task RunActivityAsync()
        {
            await DisplayStartMessageAsync();
            Random rand = new Random();
            string chosenPrompt = _prompts[rand.Next(_prompts.Count)];
            Console.WriteLine($"\nReflect on the following prompt:\n--- {chosenPrompt} ---");
            await PauseAnimationAsync(5);
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(_duration);
            while (DateTime.Now < endTime)
            {
                string question = GetNextQuestion();
                Console.WriteLine($"\n{question}");
                if (_journalingMode)
                {
                    Console.Write("Your response (type and press Enter): ");
                    string response = Console.ReadLine() ?? string.Empty;
                    _journalEntries.Add($"{DateTime.Now}: Q: {question} A: {response}");
                }
                else
                {
                    await PauseAnimationAsync(5);
                }
            }
            await DisplayEndMessageAsync();
            SaveReflectionSession();
        }

        // Saves your reflective insights to a text file.
        private void SaveReflectionSession()
        {
            _sessionContent = string.Join(Environment.NewLine, _journalEntries);
            string filename = $"Reflect_{DateTime.Now:MM_dd_yyyy__HHmmss}.txt";
            File.WriteAllLines(filename, _journalEntries);
            Console.WriteLine($"\nReflection session saved to file: {filename}");
        }
    }
}