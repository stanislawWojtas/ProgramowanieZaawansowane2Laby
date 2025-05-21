class Program{

    static Boolean running = true;
    static Mutex mutex = new Mutex();
    static int startedThreads = 0;

    static void Threading(int threadId){
        mutex.WaitOne();
        startedThreads++;
        Console.WriteLine($"Wątek nr: {threadId} rozpoczął działanie");
        mutex.ReleaseMutex();
        
        // Wątek czeka na uruchomienie wszystkich wątków
        while(running){
            Thread.Sleep(100);
        }
    }

    static void Main(){
        int n = 100;
        Thread[] threads = new Thread[n];

        for(int i = 0; i < n; i++){
            int threadId = i;
            threads[i] = new Thread(() => Threading(threadId));
            threads[i].Start();
        }
        // Czekamy na uruchomienie wszystkich wątków
        while(startedThreads < n){
            Thread.Sleep(100);
        }

        System.Console.WriteLine("Wszystkie wątki uruchomione, rozpoczynamy działanie");

        // Zatrzymanie wszystkich wątków
        running = false;

        //czekanie na zakończenie wszystkich wątków
        for(int i = 0; i < n; i++){
            threads[i].Join();
        }
        Console.WriteLine("Wszystkie wątki zakończyły działanie");
    }
}