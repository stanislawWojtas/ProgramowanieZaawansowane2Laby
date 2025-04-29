using System;

namespace TransakcjeBankowe{
    class Program{
        static void Main(){
            //Przetestowanie działania programu
            OsobaFizyczna osoba1 = new OsobaFizyczna("Jan", "Kowalski", "Adam", "12345678901", "123456");
            OsobaFizyczna osoba2 = new OsobaFizyczna("Maciej", "Nowak", "Jan", "12345678902", "023456");
            
            OsobaPrawna firma = new OsobaPrawna("Polex", "ul. strzelców wielickich 12, Wieliczka");

            List<PosiadaczRachunku> posiadacze1 = new List<PosiadaczRachunku>{osoba1, firma};
            List<PosiadaczRachunku> posiadacze2 = new List<PosiadaczRachunku>{osoba2};

            // Stworzenie dwóch rachunków(jeden to firma z właścicielem osoba1 a drugi to osoba fizyczna osoba2)
            RachunekBankowy rachunek1 = new RachunekBankowy("111111111111", 1000, true, posiadacze1);
            RachunekBankowy rachunek2 = new RachunekBankowy("222222222222", 500, false, posiadacze2);

            //przelew z konta firmy na konto klienta
            System.Console.WriteLine("Przelew z firmy na rachunek osoby2 (600)");
            RachunekBankowy.DokonajTransakcji(rachunek1, rachunek2, 600, "Wypłata za marzec");
            System.Console.WriteLine($"Stan konta firmy i osoby1: {rachunek1.StanRachunku}");
            System.Console.WriteLine($"Stan konta osoby2: {rachunek2.StanRachunku}");
            System.Console.WriteLine();

            //wypłata środków przez osobe2
            System.Console.WriteLine("Wypłata środków przez osobe2 (1000)");
            RachunekBankowy.DokonajTransakcji(rachunek2, null, 1000, "Wypłata gotówki");
            System.Console.WriteLine($"Stan konta osoby2: {rachunek2.StanRachunku}");
            System.Console.WriteLine();
    
            //wpłata środków na konto firmy
            System.Console.WriteLine("Wpłata środków na konto firmy (600)");
            RachunekBankowy.DokonajTransakcji(null, rachunek1, 600, "Wpłata gotówki");
            System.Console.WriteLine($"Stan konta firmy i osoby1: {rachunek1.StanRachunku}");
            System.Console.WriteLine();

            //przelew na konto klienta i firma jest na minusie (oszustwa podatkowe)
            System.Console.WriteLine("Przelew na konto klienta i firma jest na minusie (2000)");
            RachunekBankowy.DokonajTransakcji(rachunek1, rachunek2, 2000, "Oszustwa podatkowe");
            System.Console.WriteLine($"Stan konta firmy i osoby1: {rachunek1.StanRachunku}");
            System.Console.WriteLine($"Stan konta osoby2: {rachunek2.StanRachunku}");
            System.Console.WriteLine();

            //osoba 2 chce wypłacić więcej niż ma na koncie
            System.Console.WriteLine("Osoba 2 chce wypłacić więcej niż ma na koncie (3000)");
            RachunekBankowy.DokonajTransakcji(rachunek2, null, 3000, "Wypłata gotówki (za dużo)");
            System.Console.WriteLine($"Stan konta osoby2: {rachunek2.StanRachunku}");
            System.Console.WriteLine();

        }
    }
}