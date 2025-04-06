class Program{
    
    static Boolean running = true;

    static void MonitorDir(string dicPath){

        // s - nadawca wydarzenia, e - event w funkcji hadnlera, czyli jak pojawi się plik to wykona się zdarzenie
        FileSystemWatcher watcher = new FileSystemWatcher(dicPath);
        watcher.Created += (s, e) => {
            System.Console.WriteLine($"dodano plik {e.Name}");
        };
        watcher.Deleted += (s, e) => {
            System.Console.WriteLine($"usunięto plik {e.Name}");
        };

        // włączenie monitorowania
        watcher.EnableRaisingEvents = true;
        while(running){
            Thread.Sleep(100);
        }

    }
    static void Main(){
        string path = "monitored";
        Thread monitorThread = new Thread(() => MonitorDir(path));
        monitorThread.Start();

        while(running){
            if(Console.KeyAvailable){
                // intercept = true powoduje że klawisz nie zostanie wyświetlony w konsoli
                var key = Console.ReadKey(intercept: true).Key;
                if(key == ConsoleKey.Q){
                    running = false;
                }
            }
        }
        running = false;
        monitorThread.Join();
    }
}