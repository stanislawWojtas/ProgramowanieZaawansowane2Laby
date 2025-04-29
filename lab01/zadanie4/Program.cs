Console.Write("Podaj nazwe pliku: ");
string filename = Console.ReadLine();
Console.Write("Podaj liczbe n: ");
int n = int.Parse(Console.ReadLine());
Console.Write("Podaj dolną granicę przedziału: ");
double a = double.Parse(Console.ReadLine());
Console.Write("Podaj górną granicę przedziału: ");
double b = double.Parse(Console.ReadLine());
Console.Write("Podaj seed: ");
int seed = int.Parse(Console.ReadLine());
Console.Write("Czy liczby mają być całkowite? (t/n): ");
bool isInt = (Console.ReadLine() == "t");

Random rand = new Random(seed);
StreamWriter sw = new StreamWriter(filename);
for(int i = 0; i < n; i++){
    if(isInt){
        sw.WriteLine(rand.Next((int)a, (int)b));
    }else{
        sw.WriteLine(rand.NextDouble() * (b - a) + a);
    }
}   
sw.Close();