using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Obligatorisk_Opgave_4_5
{
    public class TcpServer
    {
        public async Task StartAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 7);
            listener.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                System.Net.Sockets.TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected...");
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }

        private async Task HandleClientAsync(System.Net.Sockets.TcpClient client)
        {
            using NetworkStream ns = client.GetStream();
            using StreamReader reader = new StreamReader(ns);
            using StreamWriter writer = new StreamWriter(ns) { AutoFlush = true };

            writer.WriteLine("Welcome to the server! Enter command (Random, Add, Subtract):");
            string command = await reader.ReadLineAsync();
            Console.WriteLine($"Received command: {command}");

            writer.WriteLine("Input numbers");
            string numbers = await reader.ReadLineAsync();
            Console.WriteLine($"Received numbers: {numbers}");
            string[] parts = numbers.Split(' ');
            int num1 = int.Parse(parts[0]);
            int num2 = int.Parse(parts[1]);

            string result = command switch
            {
                "Random" => new Random().Next(num1, num2 + 1).ToString(),
                "Add" => (num1 + num2).ToString(),
                "Subtract" => (num1 - num2).ToString(),
                _ => "Invalid command"
            };

            writer.WriteLine(result);
            Console.WriteLine($"Sent result: {result}");

            client.Close();
        }
    }
}