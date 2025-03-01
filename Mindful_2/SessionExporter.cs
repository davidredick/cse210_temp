using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MindfulnessProgram
{
    // SessionExporter: The class that ensures your insightful sessions are safely exported.
    public static class SessionExporter
    {
        // Handles exporting sessions with robust error checking and a touch of humor.
        public static async Task HandleExportAsync(
            List<(DateTime timestamp, string content)> reflectionSessions,
            List<(DateTime timestamp, string content)> listingSessions)
        {
            Console.Clear();
            Console.WriteLine("=== Export Sessions ===");
            Console.WriteLine("Do you really want to export? (Because exporting is serious business) (y/n): ");
            string exportChoice = (Console.ReadLine() ?? string.Empty).Trim().ToLower();
            if (exportChoice != "y" && exportChoice != "yes")
            {
                Console.WriteLine("Export aborted. Your sessions remain safely in limbo.");
                PauseBeforeMenu();
                return;
            }
            string defaultExportDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Console.WriteLine($"\nThe default export location is your Desktop: {defaultExportDir}");
            Console.Write("Do you want to change it? (y/n): ");
            string changeDir = (Console.ReadLine() ?? string.Empty).Trim().ToLower();
            string exportDir = defaultExportDir;
            if (changeDir == "y" || changeDir == "yes")
            {
                Console.Write("Enter the full directory path to export to: ");
                exportDir = Console.ReadLine() ?? string.Empty;
                if (!Directory.Exists(exportDir))
                {
                    Console.WriteLine("Directory does not exist. Creating it because we believe in second chances.");
                    Directory.CreateDirectory(exportDir);
                }
            }
            Console.Write("Enter the output file name (without extension): ");
            string outputFileName = (Console.ReadLine() ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(outputFileName))
            {
                outputFileName = "ExportedSessions";
            }
            outputFileName += ".txt";
            Console.WriteLine("\nWhat would you like to export?");
            Console.WriteLine("1. Reflection Sessions");
            Console.WriteLine("2. Listing Sessions");
            Console.Write("Choose (1-2): ");
            string typeChoice = (Console.ReadLine() ?? string.Empty).Trim();
            try
            {
                if (typeChoice == "1")
                {
                    await ExportMergedSessionsAsync(reflectionSessions, outputFileName, exportDir, "Reflection");
                }
                else if (typeChoice == "2")
                {
                    await ExportMergedSessionsAsync(listingSessions, outputFileName, exportDir, "Listing");
                }
                else
                {
                    Console.WriteLine("Invalid export option selected. Export canceled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during export: {ex.Message}");
            }
            PauseBeforeMenu();
        }

        // Merges all sessions and writes them to the specified file.
        private static async Task ExportMergedSessionsAsync(
            List<(DateTime timestamp, string content)> sessions,
            string fileName,
            string directory,
            string sessionType)
        {
            if (sessions.Count == 0)
            {
                Console.WriteLine($"No {sessionType} sessions to export. Maybe next time you'll be more productive?");
                return;
            }
            sessions.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
            string fullPath = System.IO.Path.Combine(directory, fileName);
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                await writer.WriteLineAsync($"=== Merged {sessionType} Sessions Export ===");
                await writer.WriteLineAsync("");
                foreach (var session in sessions)
                {
                    await writer.WriteLineAsync($"--- {session.timestamp:MM/dd/yyyy HH:mm:ss} ---");
                    await writer.WriteLineAsync(session.content);
                    await writer.WriteLineAsync(new string('-', 50));
                    await writer.WriteLineAsync("");
                }
            }
            Console.WriteLine($"Export successful! Merged {sessionType} sessions have been exported to: {fullPath}");
        }

        // A brief pause before returning to the menu.
        private static void PauseBeforeMenu()
        {
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }
    }
}