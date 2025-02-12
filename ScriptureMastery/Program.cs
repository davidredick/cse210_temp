// Date/Time: 2025-02-04 12:11:30
// The Program class: Where the magic (and a bit of irreverent humor) happens!
// This is the entry point for the ScriptureMastery project.
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScriptureMastery
{
    public class Program
    {
        // Our library of 10 random scriptures—because variety is the spice of life (and memorization)!
        private static List<Scripture> scriptureLibrary = new List<Scripture>()
        {
            new Scripture(
                new Reference("John", 3, 16),
                "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life."
            ),
            new Scripture(
                new Reference("Proverbs", 3, 5, 6),
                "Trust in the LORD with all your heart and lean not on your own understanding;\nIn all your ways submit to him, and he will make your paths straight."
            ),
            new Scripture(
                new Reference("Psalm", 23, 1),
                "The LORD is my shepherd, I lack nothing."
            ),
            new Scripture(
                new Reference("Philippians", 4, 13),
                "I can do all this through him who gives me strength."
            ),
            new Scripture(
                new Reference("Romans", 8, 28),
                "And we know that in all things God works for the good of those who love him, who have been called according to his purpose."
            ),
            new Scripture(
                new Reference("Jeremiah", 29, 11),
                "For I know the plans I have for you, declares the LORD, plans to prosper you and not to harm you,\nplans to give you hope and a future."
            ),
            new Scripture(
                new Reference("Isaiah", 41, 10),
                "So do not fear, for I am with you; do not be dismayed, for I am your God."
            ),
            new Scripture(
                new Reference("Matthew", 11, 28),
                "Come to me, all you who are weary and burdened, and I will give you rest."
            ),
            new Scripture(
                new Reference("1 Corinthians", 13, 4, 7),
                "Love is patient, love is kind. It does not envy, it does not boast, it is not proud."
            ),
            new Scripture(
                new Reference("Hebrews", 11, 1),
                "Now faith is confidence in what we hope for and assurance about what we do not see."
            )
        };

        public static void Main(string[] args)
        {
            PrintWithDate("Welcome to ScriptureMastery! Get ready for some holy hide-and-seek with your memory.");

            bool exitProgram = false;
            Scripture currentScripture = null;
            Random random = new Random();

            while (!exitProgram)
            {
                // Pick a scripture if none is currently loaded.
                if (currentScripture == null)
                {
                    currentScripture = GetRandomScripture(random);
                }

                Console.Clear();
                PrintWithDate("Here is your scripture:");
                PrintWithDate(currentScripture.GetRenderedText());

                if (currentScripture.IsCompletelyHidden())
                {
                    PrintWithDate("Amazingly, all words are hidden! (Or maybe you just outsmarted the program?)");
                }

                // Present menu options.
                PrintWithDate("Options:");
                PrintWithDate("1) Redo this scripture (reset and start over)");
                PrintWithDate("2) Next scripture (if variety tickles your fancy)");
                PrintWithDate("3) Quit (if you’ve had enough divine distraction)");

                PrintWithDate("Enter your choice (press Enter to hide a few more words):");
                string input = Console.ReadLine()?.Trim().ToLower();

                if (input == "3" || input == "quit")
                {
                    PrintWithDate("Exiting ScriptureMastery. May your memory shine as bright as your spirit!");
                    exitProgram = true;
                }
                else if (input == "1" || input == "redo")
                {
                    currentScripture.Reset();
                    PrintWithDate("Redoing the scripture. Fresh start—because even saints need a do-over!");
                    PauseBriefly();
                }
                else if (input == "2" || input == "next")
                {
                    currentScripture = GetRandomScripture(random);
                    PrintWithDate("Switching scriptures. New words, new wisdom!");
                    PauseBriefly();
                }
                else if (string.IsNullOrEmpty(input))
                {
                    // Hide a few random words (3 at a time, because slow and steady wins the race).
                    currentScripture.HideRandomWords(3);

                    // If all words become hidden, display the final state and ask for further action.
                    if (currentScripture.IsCompletelyHidden())
                    {
                        Console.Clear();
                        PrintWithDate("Final Revelation (or complete concealment):");
                        PrintWithDate(currentScripture.GetRenderedText());
                        PrintWithDate("All words are hidden! Type 1 to redo, 2 for next scripture, or 3 to quit.");
                        string finalChoice = Console.ReadLine()?.Trim().ToLower();

                        if (finalChoice == "1" || finalChoice == "redo")
                        {
                            currentScripture.Reset();
                            PrintWithDate("Scripture reset. Let the game begin anew!");
                            PauseBriefly();
                        }
                        else if (finalChoice == "2" || finalChoice == "next")
                        {
                            currentScripture = GetRandomScripture(random);
                            PrintWithDate("Enjoy your new scripture. May the hidden words challenge you further!");
                            PauseBriefly();
                        }
                        else if (finalChoice == "3" || finalChoice == "quit")
                        {
                            PrintWithDate("Exiting ScriptureMastery. Thanks for joining this sacred memory exercise!");
                            exitProgram = true;
                        }
                    }
                }
                else
                {
                    // Unrecognized input: a polite nudge to try again.
                    PrintWithDate("Hmm... that doesn't look like a valid option. Please try again, dear memorizer!");
                    PauseBriefly();
                }
            }
        }

        // Returns a deep copy of a randomly selected scripture from the library.
        private static Scripture GetRandomScripture(Random random)
        {
            int index = random.Next(scriptureLibrary.Count);
            Scripture selected = scriptureLibrary[index];
            // Create a new instance to ensure we start with an unhidden scripture.
            return new Scripture(
                ExtractReference(selected),
                ExtractText(selected)
            );
        }

        // Helper method to print messages with a date/time stamp.
        private static void PrintWithDate(string message)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
        }

        // A brief pause to add dramatic effect. Because sometimes, even divinity needs a beat.
        private static void PauseBriefly()
        {
            System.Threading.Thread.Sleep(1000);
        }

        // Extracts the Reference from a Scripture by parsing its rendered text.
        // (Yes, it's a hack—but it works, and sometimes hacks are just creative solutions!)
        private static Reference ExtractReference(Scripture scripture)
        {
            string rendered = scripture.GetRenderedText();
            string firstLine = rendered.Split('\n')[0];
            var parts = firstLine.Split(' ');

            string book = parts[0];
            string chapterAndVerse = parts[1];

            if (chapterAndVerse.Contains("-"))
            {
                var chapterSplit = chapterAndVerse.Split(':');
                int chapter = int.Parse(chapterSplit[0]);
                var verses = chapterSplit[1].Split('-');
                int startVerse = int.Parse(verses[0]);
                int endVerse = int.Parse(verses[1]);
                return new Reference(book, chapter, startVerse, endVerse);
            }
            else
            {
                var chapterSplit = chapterAndVerse.Split(':');
                int chapter = int.Parse(chapterSplit[0]);
                int verse = int.Parse(chapterSplit[1]);
                return new Reference(book, chapter, verse);
            }
        }

        // Extracts the scripture text (minus the reference) from a Scripture.
        private static string ExtractText(Scripture scripture)
        {
            string rendered = scripture.GetRenderedText();
            string[] lines = rendered.Split('\n');
            if (lines.Length >= 2)
            {
                // Rejoin the text parts preserving original whitespace
                return string.Join("", lines.Skip(1));
            }
            return "";
        }
    }
}
