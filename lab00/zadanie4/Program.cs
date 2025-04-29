using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<string> dzwieki = new List<string> { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "B", "H" };
        List<int> interwaly = new List<int> { 2, 2, 1, 2, 2, 2, 1 };

        Console.WriteLine("Podaj dźwięk początkowy:");
        string startDzwiek = Console.ReadLine();

        if (!dzwieki.Contains(startDzwiek))
        {
            Console.WriteLine("Niepoprawny dźwięk.");
            return;
        }

        List<string> gama = new List<string>();
        int startIndex = dzwieki.IndexOf(startDzwiek);
        int currentIndex = startIndex;

        gama.Add(dzwieki[currentIndex]);

        foreach (int interval in interwaly)
        {
            currentIndex = (currentIndex + interval) % dzwieki.Count;
            gama.Add(dzwieki[currentIndex]);
        }

        Console.WriteLine("Gama dur dla dźwięku " + startDzwiek + ":");
        foreach (string dzwiek in gama)
        {
            Console.Write(dzwiek + " ");
        }
    }
}