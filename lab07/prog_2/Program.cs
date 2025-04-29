using System.Security.Cryptography;
using System.Text;

class Program{
    public static void Main(string[] args){
        if(args.Length != 3){
            throw new ArgumentException("Nieprawidłowa ilość argumenów");
        }
        if(!File.Exists(args[0])){
            throw new FileNotFoundException($"Nie można odnaleźć pliku {args[0]}");
        }
        string encoding = args[2];
        if(encoding.Equals("SHA256")){
            CheckHash(args[0], args[1], encoding);
        }
        else if(encoding.Equals("SHA512")){
            CheckHash(args[0], args[1], encoding);
        }
        else if(encoding.Equals("MD5")){
            CheckHash(args[0], args[1], encoding);
        }
        else{
            throw new ArgumentException("Nieprawidłowa nazwa kodowania");
        }
    }

    public static void CheckHash(string sourceFile, string targetFile, string encoding){
        String hash = calculateHash(File.ReadAllText(sourceFile), encoding);
        if(!File.Exists(targetFile)){
            File.WriteAllText(targetFile, hash);
        }
        else{
            String textFile = File.ReadAllText(targetFile);
            if(textFile.Equals(hash)){
                Console.WriteLine("Hash jest zgodny");
            }
            else{
                Console.WriteLine("Hash nie jest zgodny z tym w pliku!");
            }
        }
    }

    public static String calculateHash(String text, string encoding){

        Encoding enc = Encoding.UTF8; 
        var hashBuilder = new StringBuilder();
        if(encoding.Equals("SHA256")){
            using var hash = SHA256.Create();
            byte[] result = hash.ComputeHash(enc.GetBytes(text));
            foreach (var b in result){
                hashBuilder.Append(b.ToString("x2"));
            }
        }
        else if(encoding.Equals("SHA512")){
            using var hash = SHA512.Create();
            byte[] result = hash.ComputeHash(enc.GetBytes(text));
            foreach (var b in result){
                hashBuilder.Append(b.ToString("x2"));
            }
        }
        else if(encoding.Equals("MD5")){
            using var hash = MD5.Create();
            byte[] result = hash.ComputeHash(enc.GetBytes(text));
            foreach (var b in result){
                hashBuilder.Append(b.ToString("x2"));
            }
        }
        return hashBuilder.ToString();
    }
}
