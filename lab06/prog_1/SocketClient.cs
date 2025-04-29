using System.Net;
using System.Net.Sockets;
using System.Text;

class SocketClient{
    public static int messageSize = 15;
    public static void Start(){
        IPHostEntry host = Dns.GetHostEntry("localhost");
        //wybieramy pierwszy adres z listy
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        Socket socket = new(
            localEndPoint.AddressFamily, 
            SocketType.Stream, 
            ProtocolType.Tcp);

        //łączenie się z serwerem
        socket.Connect(localEndPoint);
        //wysyłanie wiadomości na serwer enkodowanej w UTF8
        Thread.Sleep(100);
        Console.WriteLine("C:   Podaj wiadomość do wysłania: ");
        string wiadomosc = Console.ReadLine() ?? string.Empty;
        byte[] wiadomoscBajty = Encoding.UTF8.GetBytes(wiadomosc);
        //wysłanie tylko pierwszych 1024 bajtów
        socket.Send(wiadomoscBajty, 0, Math.Min(wiadomoscBajty.Length, messageSize), SocketFlags.None);
        //bufor na odbieranie danych
        var bufor = new byte[messageSize];
        //odebranie wiadomosci z serwera
        int numberOfBytes = socket.Receive(bufor, SocketFlags.None);
        String odpowiedzSerwera = Encoding.UTF8.GetString(bufor, 0, numberOfBytes);
        Console.WriteLine($"C:  Odpowiedz servera: \"{odpowiedzSerwera}\"");
        try {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch{}
        Console.WriteLine("C:   Client has ended his life");
    }
}
