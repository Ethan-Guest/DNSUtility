// See https://aka.ms/new-console-template for more information

using DNSBench_Console_App;
using DNSUtility.Service.Parsers;

namespace MyApp;
// Note: actual namespace depends on the project name.

internal class Program
{
    public static bool exitConditon;

    // User
    private static readonly User currentUser = new();

    public static void MainLoop()
    {
        Console.WriteLine("Welcome to DNS Bench!");

        Console.WriteLine("What operation would you like to perform?");

        Console.WriteLine("1 = Check my DNS");

        Console.WriteLine("2 = Perform DNS test");

        Console.WriteLine("3 = Change DNS");

        Console.WriteLine("4 = Exit");

        var input = Console.ReadLine();

        // Check user's current DNS
        if (input == "1")
        {
            currentUser.Initialize();
            Console.WriteLine("Your current primary DNS address: " + currentUser.primaryDNS);
            Console.WriteLine("Your current secondary DNS address: " + currentUser.secondaryDNS);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        // Run DNS benchmark
        else if (input == "2")
        {
            var parser = new NameserverCsvParser();
            var results = parser.Parse("https://public-dns.info/nameservers.csv")
                .Where(n => n.Country == "US" && n.Dnssec && n.Reliability == 1)
                .ToList();

            // Instantiate the nameserver class
            var nameservers = new Nameservers();

            Console.Clear();

            // Run tests
            for (var i = 0; i < nameservers.TESTSTORUN; i++)
            {
                Console.WriteLine($"[RUNNING TEST ON {results.Count} NAMESERVERS");
                Console.WriteLine(
                    $"Host:{Nameservers.spaces.Remove(0, 6)} Address: {Nameservers.spaces.Remove(0, 9)}Ping:");

                nameservers.RunPingTest(results);
                Thread.Sleep(100);
                Console.Clear();
            }

            Console.WriteLine("[TEST COMPLETE]");

            nameservers.DisplayResults(results);


            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        else if (input == "3")
        {
            Console.Write("Enter your new primaryDNS: ");
            //userDns = Console.ReadLine();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        else if (input == "4")
        {
            exitConditon = true;
            return;
        }

        Console.Clear();
    }

    private static void Main(string[] args)
    {
        while (!exitConditon) MainLoop();
    }
}