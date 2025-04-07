using System.Net;

class Program{
    static Mutex mutex = new Mutex();
    static Queue<string> foundFiles = new Queue<string>();
    static Boolean running = true;

    static void SearchForFiles(string path, string word){
        try{
            foreach (string file in Directory.GetFiles(path)){
                string fileName = Path.GetFileName(file);
                if(fileName.Contains(word)){
                    mutex.WaitOne();
                    foundFiles.Enqueue(fileName);
                    mutex.ReleaseMutex();
                }
            }
            
            // rekurencyjne wywołanie na podfolderach
            foreach (string directory in Directory.GetDirectories(path)){
                SearchForFiles(directory, word);
            }
        }
        catch(Exception e){
            System.Console.WriteLine($"Error: {e.Message}");
        }
        // po przejściu przez wszystkie pliki ustawiamy running na false
        finally{
            running = false;
        }
    }
    static void Main(){
        string path = "folder";
        
        Thread searchThread = new Thread(() => SearchForFiles(path, "ra"));
        searchThread.Start();

        while(running){
            Thread.Sleep(1000);
        }
        searchThread.Join();

        foreach (string file in foundFiles){
            System.Console.Write("Znaleziono plik: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(file);
            Console.ResetColor();
        }

    }
}