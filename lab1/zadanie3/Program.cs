Console.Write("Podaj nazwę pliku: ");
string filename = Console.ReadLine();
Console.Write("Podaj szukany ciąg: ");
string search = Console.ReadLine();
bool found = false;
StreamReader sr = new StreamReader(filename);
int line_num = 1; //zakładam że linie zaczynają się od 1
while(!sr.EndOfStream){
    string line = sr.ReadLine();
    //metoda od razu daje nam index ciągu (jeśli nie ma to zwraca -1)
    int index = line.IndexOf(search);
    if(index != -1){
        Console.WriteLine($"Znaleziono ciąg w linii numer: {line_num}, na pozycji: {index}");
        found = true;
    }
}
if(!found){
    Console.WriteLine("Nie znaleziono ciągu w pliku");
}
sr.Close();