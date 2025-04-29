using System.Linq; //potrzebne do Min i Max
using System;

string filename = "../zadanie4/test.txt";
StreamReader sr = new StreamReader(filename);
int line_count = 0;
int char_count = 0;

//pierwszą linie pobieram ręcznie aby sprawdzić czy liczby są typu double czy int
string line = sr.ReadLine();
bool isDouble = line.Contains(",");
if(isDouble){ //liczby double
    List<double> numbers = new List<double>();
    numbers.Add(double.Parse(line));
    char_count += line.Length;
    line_count++;
    while(!sr.EndOfStream){
        line = sr.ReadLine();
        numbers.Add(double.Parse(line));
        char_count += line.Length;
        line_count++;
    }
    double min = numbers.Min();
    double max = numbers.Max();
    double avg = numbers.Average();

    //podsumowanie wynikow
    Console.WriteLine($"Liczba linii: {line_count}");
    Console.WriteLine($"Liczba znaków: {char_count}");
    Console.WriteLine($"Min: {min}");
    Console.WriteLine($"Max: {max}");
    Console.WriteLine($"Średnia: {avg}");
}
else{ 
    List<int> numbers = new List<int>();
    numbers.Add(int.Parse(line));
    char_count += line.Length;
    line_count++;
    while(!sr.EndOfStream){
        line = sr.ReadLine();
        numbers.Add(int.Parse(line));
        char_count += line.Length;
        line_count++;
    }
    int min = numbers.Min();
    int max = numbers.Max();
    double avg = numbers.Average();

    //Podsumowanie wyników
    Console.WriteLine($"Liczba linii: {line_count}");
    Console.WriteLine($"Liczba znaków: {char_count}");
    Console.WriteLine($"Min: {min}");
    Console.WriteLine($"Max: {max}");
    Console.WriteLine($"Średnia: {avg}");
}
sr.Close();