StreamReader sr = new StreamReader("PlikZLiczbami.txt");
int cur_line = 1;
int max_line = 0;
int max_val = 0;
while (!sr.EndOfStream)
{
    String napis = sr.ReadLine();
    string[] words = napis.Split(' ');
    foreach(string word in words){
        if(int.TryParse(word, out int number)){
            if(number > max_val){
                max_line = cur_line;
                max_val = number;
            }
        }
    }
    cur_line++;
}
sr.Close();

Console.WriteLine("Największa liczba znajduje się w linii: " + max_line + " i wynosi: " + max_val);
