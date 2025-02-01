
using System;
using System.Collections.Generic;

// Yeah, this is the fancy class where we keep someone's life story. 
public class Resume
{
    public string _name; // The poor soul whose resume this is.
    public List<Job> _jobs = new(); // Because one job isn't enough, right?

    // Show it all. Every single job. No hiding here.
    public void Display()
    {
        Console.WriteLine($"Name: {_name}");
        Console.WriteLine("Jobs:");
        foreach (var job in _jobs)
        {
            job.Display();
        }
    }
}
