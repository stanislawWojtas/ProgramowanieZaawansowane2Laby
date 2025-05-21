using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using System.Text;

class SocketClient{
    public static void Start(){
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        Socket socket = new (localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        socket.Connect(localEndPoint);

        Thread.Sleep(100);

        // Wypisanie wiadomości
        Console.WriteLine("C:   Enter a message: ");
        string message = Console.ReadLine() ?? "";

        //Obliczenie i wysłanie długości wiadomości
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);

        // Wysłanie najpierw długości
        socket.Send(lengthBytes);

        // Wysłanie prawdziwej wiadomości
        socket.Send(messageBytes);


        // odebranie długości wiadomości zwrotnej
        byte[] receivedLengthBytes = new byte[4];
        socket.Receive(receivedLengthBytes);

        // Odebranie wiadomości zwrotnej
        byte[] serverResponse = new byte[BitConverter.ToInt32(receivedLengthBytes, 0)];
        socket.Receive(serverResponse);
        string response = Encoding.UTF8.GetString(serverResponse);

        Console.WriteLine($"C:   Received from Server: \"{response}\"");

        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
        Console.WriteLine("C:   Client has ended his life");
    }
}