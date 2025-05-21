using System.Runtime.Serialization;
using System.Security.Cryptography;

class Program{
    const string filePublicKey = "publicKey.dat";
    const string filePrivateKey = "privateKey.dat";
    public static void Main(string[] args){
        if(args.Length != 2){
            throw new ArgumentException("Nieprawidłowa ilość argumentów");
        }
        if(!File.Exists(args[0])){
            throw new ArgumentException($"Plik o podanej nazwie: {args[0]} nie istnieje");
        }

        if(!File.Exists(filePrivateKey) && !File.Exists(filePublicKey)){
            GenerateKeys();
        }
        
        string sourceFile = args[0];
        string targetFile = args[1];

        using SHA256 hashAlg = SHA256.Create();
        byte[] data = File.ReadAllBytes(sourceFile);
        byte[] hash = hashAlg.ComputeHash(data);


        if(!File.Exists(targetFile)){
            /// tworzenie podpisu
            string privateKey = File.ReadAllText(filePrivateKey);

            using RSA rsa = RSA.Create();
            rsa.FromXmlString(privateKey);

            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm(nameof(SHA256));
            
            byte[] signature = rsaFormatter.CreateSignature(hash);
            File.WriteAllBytes(targetFile, signature);
            Console.WriteLine("Podpis został wygenerowany i zapisany");
        }
        else{
            /// weryfikacja podpisu
            byte[] signature = File.ReadAllBytes(targetFile);
            string publicKey = File.ReadAllText(filePublicKey);

            using RSA rsa = RSA.Create();
            rsa.FromXmlString(publicKey);

            RSAPKCS1SignatureDeformatter rsaVerifier = new RSAPKCS1SignatureDeformatter();
            rsaVerifier.SetKey(rsa);
            rsaVerifier.SetHashAlgorithm(nameof(SHA256));

            bool isValid = rsaVerifier.VerifySignature(hash, signature);
            if(isValid){
                Console.WriteLine("Podpis jest prawidłowy");
            }
            else{
                Console.WriteLine("Podpis jest błędny!!!!");
            }
        }

    }

    public static void GenerateKeys(){
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        File.WriteAllText(filePublicKey, rsa.ToXmlString(false));
        File.WriteAllText(filePrivateKey, rsa.ToXmlString(true));
        Console.WriteLine("Klucze zostały wygenerowane i zapisane do plików");
    }
}
