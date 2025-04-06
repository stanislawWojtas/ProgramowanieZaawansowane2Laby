using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;

class Program{

    static Mutex mutex = new Mutex();

    static Queue<Data> queue = new Queue<Data>();
    static Boolean running = true;
    static Dictionary<int, Dictionary<int, int>> results = new Dictionary<int, Dictionary<int, int>>();
    
    class Producer{
        public int Number;
        private Random rand = new Random();
        public int Sleep;

        public Producer(int number, int sleep){
            Number = number;
            Sleep = sleep;

        }

        public void Start(){
            // dopóki program działa
            while(running){
                Thread.Sleep(rand.Next(Sleep));
                mutex.WaitOne();
                Data data = new Data(Number);
                queue.Enqueue(data);
                System.Console.WriteLine($"Producent {Number} dodał dane");
                mutex.ReleaseMutex();
            }
        }
    }

    class Consumer{
        public int Number;
        private Random rand = new Random();
        public int Sleep;

        public Consumer(int number, int sleep){
            Number = number;
            Sleep = sleep;
        }

        public void Start(){
            while(running){
                mutex.WaitOne();
                if(queue.Count > 0){
                    var data = queue.Dequeue();
                    System.Console.WriteLine($"Konsument {Number} skonsumował dane");
                    // dodanie do słownika skonsumowanego itemu
                    results[Number][data.ProducerNumber] += 1;
                    mutex.ReleaseMutex();
                    Thread.Sleep(rand.Next(Sleep));

                }
                else{
                    mutex.ReleaseMutex();
                }
                  
            }
        }

    }

    class Data{
        public int ProducerNumber {get;}
        
        public Data(int producerNumber){
            ProducerNumber = producerNumber;
        }
    }

    static void Main(){
        int n = 5;
        int m = 3;
        int producerSleep = 2000;
        int consumerSleep = 3000;
        List<Thread> producerThreads = new List<Thread>();
        List<Thread> consumerThreads = new List<Thread>();

        // inicjalizacja słownika results
        for(int i = 0; i < m; i++){
            results[i] = new Dictionary<int, int>();
            for(int j = 0; j < n; j++){
                results[i][j] = 0;
            }
        }

        for(int i = 0; i < n; i++){
            var producer = new Producer(i, producerSleep);
            var pThread = new Thread(producer.Start);
            producerThreads.Add(pThread);
            pThread.Start();
        }

        for(int i = 0; i < m; i++){
            var consumer = new Consumer(i, consumerSleep);
            var cThread = new Thread(consumer.Start);
            consumerThreads.Add(cThread);
            cThread.Start();
        }

        while(running){
            if(Console.KeyAvailable){
                // intercept = true powoduje że klawisz nie zostanie wyświetlony w konsoli
                var key = Console.ReadKey(intercept: true).Key;
                if(key == ConsoleKey.Q){
                    running = false;
                }
            }
        }



        // czekanie na zakończenie wszystkich wątków
        foreach(var thread in producerThreads){
            thread.Join();
        }
        foreach(var thread in consumerThreads){
            thread.Join();
        }


        // wypisanie wyników
        for(int i = 0; i < m; i++){
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"Wyniki dla konsument {i}");
            Console.ForegroundColor = ConsoleColor.Green;
            for(int j = 0; j < n; j++){
                System.Console.WriteLine($"Producent {j} - {results[i][j]}");
            }
            Console.ResetColor();
            System.Console.WriteLine();
        }
    }
}