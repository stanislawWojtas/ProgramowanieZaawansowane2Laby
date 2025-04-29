class Program{
    public static void Main(string[] args){
        if(args[0] == "server"){
            Server.Start();
        }
        else if(args[0] == "client"){
            Client.Start();
        }
        else{
            Console.WriteLine("Enter valid world");
        }
    }
}