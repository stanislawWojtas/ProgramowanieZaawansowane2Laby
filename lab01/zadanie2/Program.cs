string word;
List<string> words = new List<string>();
while((word = Console.ReadLine()) != "koniec!"){
    words.Add(word);
}
words.Sort();
StreamWriter sw = new StreamWriter("output.txt", append: true);
string last = words[words.Count - 1];
foreach(string w in words){
    sw.WriteLine(w);
}
sw.Close();
Console.WriteLine("Ostatni znak leksykograficzny: " + last);