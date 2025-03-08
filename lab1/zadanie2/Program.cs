string word;
List<string> words = new List<string>();
while((word = Console.ReadLine()) != "koniec!"){
    words.Add(word);
}
words.Sort();
StreamWriter sw = new StreamWriter("output.txt", append: true);
foreach(string w in words){
    sw.WriteLine(w);
}
sw.Close();