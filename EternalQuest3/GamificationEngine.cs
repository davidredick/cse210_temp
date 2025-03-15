using System;

// Displays a progress bar – a visual wink at your progress.
public class GamificationEngine
{
    public void DisplayProgressBar(int score, int threshold)
    {
        int progress = Math.Min(100, (int)(((double)score / threshold) * 100));
        Console.WriteLine($"Progress: [{new string('#', progress / 10)}{new string('-', 10 - progress / 10)}] {progress}%");
    }
}
