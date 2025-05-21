using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

class Program{

    // stała tablica bajtów używana jako sól w procesie generowania klucza
    public static byte[] salt = Encoding.UTF8.GetBytes("MyOwnSalt1234");
    public static int iterations = 100000;

    // 256 bitów
    public static int keySize = 32;

    public static void Main(string[] args){
        if(args.Length != 4){
            throw new ArgumentException("Nieprawidłowa ilość argumentów");
        }
        string sourceFile = args[0];
        string targetFile = args[1];
        
        string password = args[2];
        string type = args[3];
        if(!File.Exists(sourceFile)){
            throw new FileNotFoundException($"Nie można znaleźć pliku: {sourceFile}");
        }

        if(type == "0"){
            /// zaszyfrowanie pliku source algorytmem AOS do pliku target
            /// szyfrowanie za pomocą hasła
            EncryptFile(sourceFile, targetFile, password);
        }
        else if(type == "1"){
            /// rozszyfrowanie pliku source do pliku target algorytmem AOS
            /// klucz wygenerowany za pomocą hasła
            DecryptFile(sourceFile, targetFile, password);
        }
        else{
            throw new ArgumentException($"Typ musi być równy 0 lub 1. Podany typ: {type}");
        }
    }

    public static void EncryptFile(string sourceFile, string targetFile, string password){
        byte[] data = File.ReadAllBytes(sourceFile);

        // generowanie klucza i wektora inicjalizacyjnego IV
        using Aes aes = Aes.Create();
        using var key = new Rfc2898DeriveBytes(password, salt, iterations);
        aes.Key = key.GetBytes(keySize);
        aes.IV = key.GetBytes(aes.BlockSize / 8);

        /// szyfrowanie danych
        /// tworzony jest strumień docelowy fs i strumień kryptograficzny cs do szyfrowania danych
        using FileStream fs = new FileStream(targetFile, FileMode.Create);
        using CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write);
        // zapisanie zaszyfrowanych danych w pliku
        cs.Write(data, 0, data.Length);
        Console.WriteLine("Plik został zaszyfrowany");
    }

    public static void DecryptFile(string sourceFile, string targetFile, string password){
        try{
            // generowanie klucza i wektora
            byte[] encryptedData = File.ReadAllBytes(sourceFile);
            using Aes aes = Aes.Create();
            using var key = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = key.GetBytes(keySize);
            aes.IV = key.GetBytes(aes.BlockSize / 8);

            /// odszyfrowanie danych
            /// tworzony jest strumień pamięci ms z zaszyfrowanymi danymi oraz strumień kryptograficzny cs
            using MemoryStream ms = new MemoryStream(encryptedData);
            using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            // odszyfrowane dane są kopiowane do pliku docelowego
            using FileStream fs = new FileStream(targetFile, FileMode.Create);
            cs.CopyTo(fs);
            Console.WriteLine("Plik został odszyfrowany");
        }
        catch(Exception e){
            Console.WriteLine("BŁĄD: Hasło się nie zgadza lub plik jest uszkodzony");
        }
    }
}