string filename = "przyklad.txt";
Console.Write("Podaj szukany ciąg: ");
string searchString = Console.ReadLine();
bool found = false;
StreamReader sr = new StreamReader(filename);
int line_num = 1; //zakładam że linie zaczynają się od 1
while(!sr.EndOfStream){
    string line = sr.ReadLine();
    //metoda od razu daje nam index ciągu (jeśli nie ma to zwraca -1)
    int index = line.IndexOf(searchString);
    if(index != -1){
        while(index != -1){
            Console.WriteLine($"Znaleziono ciąg w linii numer: {line_num}, na pozycji: {index}");
            found = true;
            //jeszcze raz szukam ciągu od indexu o 1 wiekszego w tej samej linii
            index = line.IndexOf(searchString, index + 1);
        }
    }
    line_num++;
}
if(!found){
    Console.WriteLine("Nie znaleziono ciągu w pliku");
}
sr.Close();