using System;
using System.Threading.Tasks;
            
namespace Obligatorisk_Opgave_4_5
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting servers...");
            
            // Text-based protocol server on port 7
            TcpServer server = new TcpServer();
            _ = Task.Run(() => server.StartAsync());

            // JSON protocol server on port 8
            TcpServerJson server2 = new TcpServerJson();
            _ = Task.Run(() => server2.StartAsync());
            
            await Task.Delay(1000);
            
            Console.WriteLine("Servers started. Choose testing option:");
            Console.WriteLine("1. Test text-based protocol");
            Console.WriteLine("2. Test JSON protocol");
            Console.WriteLine("3. Test both protocols");
            
            string choice = Console.ReadLine();
            
            if (choice == "1" || choice == "3")
            {
                Console.WriteLine("\nStarting text-based protocol tests...");
                TcpClient client = new TcpClient();
                await client.StartAsync();
            }
            
            if (choice == "2" || choice == "3")
            {
                Console.WriteLine("\nStarting JSON protocol tests...");
                TcpClientJson client2 = new TcpClientJson();
                await client2.StartAsync();
            }
            
            Console.WriteLine("\nAll tests completed. Press any key to exit.");
            Console.ReadKey();
        }
    }
}