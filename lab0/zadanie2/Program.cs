List<int> numbers = new List<int>();
int num;
while ((num = int.Parse(Console.ReadLine())) != 0){
    numbers.Add(num);
}

//sumowanie liczb i obliczenie średniej
int sum = 0;
int count = 0;
foreach (int number in numbers){
    sum += number;
    count++;
}
double avg = (double)sum / count;
StreamWriter sw = new StreamWriter("Srednia.txt", append:true);
sw.Write("Wyliczona srednia z liczb: ");
foreach (int number in numbers){
    sw.Write(number + ", ");
}
sw.WriteLine("wynosi: " + avg);
sw.Close();