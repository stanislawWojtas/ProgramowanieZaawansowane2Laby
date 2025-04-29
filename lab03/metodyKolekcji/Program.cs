using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

public class Program{
    
    public class Tweet{
        public string Text {get; set;}
        public string UserName {get; set;}
        public string LinkToTweet {get; set;}
        public string FirstLinkUrl {get; set;}
        public DateTime CreatedAt {get; set;}
        public string TweetEmbedCode {get; set;}
    }

    static void Main(){

        // zwykłe "resources/favorite-tweets.jsonl nie działa nie wiem czemu
        string filePath = Path.Combine("resources", "favorite-tweets.jsonl");
        var tweets = ReadFromJsonl(filePath);

        //zapisannie do xml
        SaveToXML(tweets, "tweets.xml");

        // Odczyt z xml
        var list2 = ReadFromXML("tweets.xml");

        // Posortowana lista tweetów według daty
        var tweetsSortedByDate = SortByDate(tweets);
        Tweet oldestTweet = tweetsSortedByDate.First();
        Tweet newestTweet = tweetsSortedByDate.Last();
        System.Console.WriteLine($"-----------------------------------\nNajstarszy tweet\nUser: {oldestTweet.UserName}\nText: {oldestTweet.Text}\nCreatedAt: {oldestTweet.CreatedAt}");
        System.Console.WriteLine($"-----------------------------------\nNajnowszy tweet\nUser: {newestTweet.UserName}\nText: {newestTweet.Text}\nCreatedAt: {newestTweet.CreatedAt}");
        System.Console.WriteLine();

        // Słownik użytkownik -> lista jego tweetów
        var myDict = UsersToTweets(tweets);

        // Słownnik słowo -> ilość jego wsyąpień
        Dictionary<string, int> wordFreq = CalculateWordsFrequency(tweets);
        var sortedDict = wordFreq.OrderByDescending (a => a.Value);
        int i = 0;
        foreach(var word in sortedDict){
            if(word.Key.Length >= 5){
                System.Console.WriteLine($"Słowo: {word.Key}    liczba wystąpień: {word.Value}");
                i++;
            }
            if(i > 5)break;
        }

        System.Console.WriteLine();

        // Słownik słowo -> współczynnik IDF
        Dictionary<string, double> IDF = CalculateIDF(tweets);
        //metoda Take(5) bierze pierwsze 5 elementów słownika
        foreach(var kvp in IDF.Take(5)){
            System.Console.WriteLine($"Słowo: {kvp.Key}, IDF = {kvp.Value}");
        }
    }

    // zwraca listę obiektów Tweet wczytanych z pliku "path"
    public static List<Tweet> ReadFromJsonl(string path){
        List<Tweet> tweets = new List<Tweet>();
        
        //przejście po każdej linii pliku
        foreach(var line in File.ReadLines(path)){
            //
            var root = JsonDocument.Parse(line).RootElement;
            var tweet = new Tweet{
                Text = root.GetProperty("Text").ToString(),
                UserName = root.GetProperty("UserName").ToString(),
                LinkToTweet = root.GetProperty("LinkToTweet").ToString(),
                FirstLinkUrl = root.GetProperty("FirstLinkUrl").ToString(),
                // Tutaj troche zabiegów żeby sparsować taką formę "MMMM dd, yyyy 'at' hh:mmtt" do DateTime
                CreatedAt = DateTime.ParseExact(root.GetProperty("CreatedAt").ToString(), "MMMM dd, yyyy 'at' hh:mmtt", CultureInfo.InvariantCulture),
                TweetEmbedCode = root.GetProperty("TweetEmbedCode").ToString()
            };
            tweets.Add(tweet);
        }
        return tweets;
    }

    
    // Do odczytu i zapisu w formaci XML używam klasy XElement z której buduje drzewo
    public static void SaveToXML(List<Tweet> tweets, string path){
        XElement xml = new XElement("Tweets", 
            tweets.Select(tweet => new XElement("Tweet",
                new XElement("Text", tweet.Text),
                new XElement("UserName", tweet.UserName),
                new XElement("LinkToTweet", tweet.LinkToTweet),
                new XElement("FirstLinkUrl", tweet.FirstLinkUrl),
                new XElement("CreatedAt", tweet.CreatedAt),
                new XElement("TweetEmbedCode", tweet.TweetEmbedCode)
            ))
        );
        xml.Save(path);
    }

    public static List<Tweet> ReadFromXML(string path){
        XElement xml = XElement.Load(path);
        return xml.Elements("Tweet").Select(tweet => new Tweet{
            Text = (string)tweet.Element("Text"),
            UserName = (string)tweet.Element("UserName"),
            LinkToTweet = (string)tweet.Element("LinkToTweet"),
            FirstLinkUrl = (string)tweet.Element("FirstLinkUrl"),
            CreatedAt = DateTime.Parse((string)tweet.Element("CreatedAt")),
            TweetEmbedCode = (string)tweet.Element("TweetEmbedCode")
        }).ToList();
    }

    // do sortowania użyjemy metody OrderBy z LINQ
    public static List<Tweet> SortByUserName(List<Tweet> tweets){
        return tweets.OrderBy(tweet => tweet.UserName).ToList();
    }

    public static List<Tweet> SortByDate(List<Tweet> tweets){
        return tweets.OrderBy(tweet => tweet.CreatedAt).ToList();
    }

    //funkcja, która tworzy słownik użytkowników i ich tweety
    public static Dictionary<string, List<Tweet>> UsersToTweets(List<Tweet> tweets){

        Dictionary<string, List<Tweet>> usersToTweets = new Dictionary<string, List<Tweet>>();
        foreach(Tweet tweet in tweets){
            //jeżeli go nie ma jeszcze klucza to go dodajemy
            if(!usersToTweets.ContainsKey(tweet.UserName)){
                usersToTweets[tweet.UserName] = new List<Tweet>();
            }
            usersToTweets[tweet.UserName].Add(tweet);
        }
        return usersToTweets;
    }

    //funckcja obliczająca częstotliwość słów w tweetach
    public static Dictionary<string, int> CalculateWordsFrequency(List<Tweet> tweets){
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach(Tweet tweet in tweets){
            //cast i select konwertuje wynik na liste słow
            string[] words = Regex.Matches(tweet.Text, @"\b[a-zA-Z]+\b").Cast<Match>().Select(m => m.Value.ToLower()).ToArray();
            foreach(string word in words){
                if(freq.ContainsKey(word)){
                    freq[word] += 1;
                }
                else{
                    freq[word] = 1;
                }
            }
        }
        return freq;
    }

    public static Dictionary<string, double> CalculateIDF(List<Tweet> tweets){
        Dictionary<string, double> wordToIDF = new Dictionary<string, double>();
         
        int n = tweets.Count;
        // słownik, który mówi w ilu dokumentach dane słowo występuje
        Dictionary<string, int> docWithWordCount = new Dictionary<string, int>();

        foreach(Tweet tweet in tweets){
            HashSet<string> uniqueWords = Regex.Matches(tweet.Text, @"\b[a-zA-Z]+\b").Cast<Match>().Select(m => m.Value.ToLower()).ToHashSet();

            foreach(string word in uniqueWords){
                if(!docWithWordCount.ContainsKey(word)){
                    docWithWordCount[word] = 1;
                }
                else{
                    docWithWordCount[word] += 1;
                }
            }
        }

        // Teraz gdy juz mamy w ilu dokumentach występuje dane słowo można obliczyć IDF
        // IDF(t) = LOG(n/DF(t)) gdzie n to liczba dokumentów (tweetów) a df to w ilu dokumentach dane słowo wysępuje
        foreach(string key in docWithWordCount.Keys){
            wordToIDF[key] = Math.Log(n/docWithWordCount[key]);
        }

        //posortowanie słownika według wartości IDF
        wordToIDF = wordToIDF.OrderByDescending(a => a.Value).ToDictionary();
        return wordToIDF;
    }
}