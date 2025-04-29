using System.Net;
using System.Net.Sockets;
using System.Text;

class SocketServer{

    public static void Start(){
        IPHostEntry host = Dns.GetHostEntry("localhost");

        IPAddress ipAddres = host.AddressList[0];

        IPEndPoint localEndPoint = new IPEndPoint(ipAddres, 11000);

        //tworzenie obiektu Socket
        Socket socketServer = new (localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        // rezerwacja portu
        socketServer.Bind(localEndPoint);
        // rozpoczęcie nasłuchiwania - tylko dla 1 połączenia
        socketServer.Listen(1);

        Console.WriteLine("S:   Wating for connection with Client");
        Socket socketClient = socketServer.Accept();
        Console.WriteLine("S:   Connected with Client");

        // Wiadomość zawierająca 4 bajty jako długość
        byte [] lengthBytes = new byte[4];
        socketClient.Receive(lengthBytes);
        int messageLength = BitConverter.ToInt32(lengthBytes);


        // odebranie "prawdziwej" wiadomość
        byte []buffer = new byte[messageLength];
        socketClient.Receive(buffer);
        string clientMessage = Encoding.UTF8.GetString(buffer);
        Console.WriteLine("S:   Got Client message: " + clientMessage);

        // odpowiedz od servera
        string serverResponse = "odczytałem: " + clientMessage;
        byte[] echoBytes = Encoding.UTF8.GetBytes(serverResponse);
        byte[] responseLength = BitConverter.GetBytes(echoBytes.Length);

        //wysłanie wiadomości z długością odpowiedzi
        socketClient.Send(responseLength);

        //wysłanie "prawdziwej" odpowiedzi
        socketClient.Send(echoBytes, 0);
        
        socketClient.Shutdown(SocketShutdown.Both);
        socketClient.Close();
        socketServer.Close();
        Console.WriteLine("S:   Server has ended his life");
    }

}