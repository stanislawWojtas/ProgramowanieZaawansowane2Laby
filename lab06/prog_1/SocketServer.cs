using System.Net;
using System.Net.Sockets;
using System.Text;
class SocketServer{
    public static void Start(){
        IPHostEntry host = Dns.GetHostEntry("localhost");
        //wybieramy pierwszy adres z listy
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        //socket nasłuchujący na porcie TCP/IP
        Socket socketSerwera = new(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        //rezerwacja portu
        socketSerwera.Bind(localEndPoint);
        //rozpoczęcie nasłuchiwania 1 klienta
        socketSerwera.Listen(1);
        //oczekiwanie na połączenie z klientem
        Console.WriteLine("S:   Waiting for connection with Client...");
        Socket socketKlienta = socketSerwera.Accept();
        Console.WriteLine("S:   Connected with Client");

        // bufor na wiadomość, max 1024 bajty
        byte []bufor = new byte[SocketClient.messageSize];
        //instrukcja blokująca, czeka na połączenie
        int received = socketKlienta.Receive(bufor, SocketFlags.None);
        String clientMessage = Encoding.UTF8.GetString(bufor, 0, received);
        Console.WriteLine("S:   Otrzymano od klienta: " + clientMessage);

        string serverResponse =  "odczytalem: " + clientMessage;
        var echoBytes = Encoding.UTF8.GetBytes(serverResponse);
        // send response to Client
        socketKlienta.Send(echoBytes, 0);
        try {
            socketSerwera.Shutdown(SocketShutdown.Both);
            socketSerwera.Close();
        }
        catch{}
        Console.WriteLine("S:   Server has ended his life");
    }
}
