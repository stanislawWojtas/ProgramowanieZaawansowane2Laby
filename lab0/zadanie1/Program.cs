// See https://aka.ms/new-console-template for more information
string line = Console.ReadLine();
string num = Console.ReadLine();
int n = int.Parse(num);
string[] words = line.Split(' ');

foreach (string word in words){
    for(int i = 0; i < n; i++){
        Console.Write(word + " ");
    }
    Console.WriteLine();
}