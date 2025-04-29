using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    public static void Main()
    {
        // Start() the server in his own Thread
        Task.Run(() => {
            SocketServer.Start();
        });

        // Wait for server
        Thread.Sleep(1000);

        // Start() the Client
        SocketClient.Start();
    }
}