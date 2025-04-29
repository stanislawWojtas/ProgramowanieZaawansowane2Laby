using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Program{
    const string filePublicKey = "publicKey.dat";
    const string filePrivateKey = "privateKey.dat";
    public static void Main(string[] args){

        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        // generate keys
        if(args[0] == "0"){
            GenerateKeys();
        }
        else if(args[0] == "1"){
            if(args.Length != 3){
                throw new ArgumentException("Tryb \"1\" wymaga 3 argumentów");
            }
            EncryptFile(args[1], args[2]);
        }
        else if(args[0] == "2"){
            if(args.Length != 3){
                throw new ArgumentException("Tryb \"2\" wymaga 3 argumentów");
            }
            DecrypFile(args[1], args[2]);
        }
        else{
            throw new ArgumentException("Nieprawidłowy argument");
        }
    }

    public static void GenerateKeys(){
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        File.WriteAllText(filePublicKey, rsa.ToXmlString(false));
        File.WriteAllText(filePrivateKey, rsa.ToXmlString(true));
        Console.WriteLine("Klucze zostały wygenerowane i zapisane do plików");
    }

    public static void EncryptFile(string sourceFile, string targetFile){
        //sprawdzenie czy pliki z kluczami istnieją
        if(!File.Exists(filePublicKey)){
            throw new Exception($"Error: brakuje pliku z kluczem publicznym: {filePublicKey}");
        }
        if(!File.Exists(sourceFile)){
            throw new Exception($"Error: brakuje pliku źródłowego {sourceFile}");
        }

        string publicKey = File.ReadAllText(filePublicKey);

        // Zmień text na tablicę bajtów   
        UnicodeEncoding byteConverter = new UnicodeEncoding();  
        byte[] dataToEncrypt = File.ReadAllBytes(sourceFile);  

        // Utwórz tablicę bajtów, aby przechowywać w niej zaszyfrowane dane   
        byte[] encryptedData;   
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())  
        {  
            // Ustaw publiczny klicz RSA   
            rsa.FromXmlString(publicKey);  
            // Zaszyfruj dane in wstaw je do tabicy zaszyfrowaneDane
            encryptedData = rsa.Encrypt(dataToEncrypt, false);   
        }  
        // Zapisz zaszyfrowaną tablicę danych do pliku   
        File.WriteAllBytes(targetFile, encryptedData);  

        Console.WriteLine("Dane zostały zaszyfrowane");   
    }

    public static void DecrypFile(string sourceFile, string targetFile){

        if(!File.Exists(filePrivateKey)){
            throw new Exception($"Error: brakuje pliku z kluczem prywatnym: {filePrivateKey}");
        }
        if(!File.Exists(sourceFile)){
            throw new Exception($"Error: brakuje pliku źródłowego {sourceFile}");
        }

        string privateKey = File.ReadAllText(filePrivateKey);
        byte[] encryptedData = File.ReadAllBytes(sourceFile);
        
        byte[] decryptedData;

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())  
        {  
            // Set the private key of the algorithm   
            rsa.FromXmlString(privateKey);  
            decryptedData = rsa.Decrypt(encryptedData, false);   
        }  

        // Get the string value from the decryptedData byte array   
        File.WriteAllBytes(targetFile, decryptedData);
        Console.WriteLine("Dane zostały odszyfrowane");  
    }
}