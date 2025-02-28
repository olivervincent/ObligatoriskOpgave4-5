using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Obligatorisk_Opgave_4_5
{
    public class TcpClient
    {
        public async Task StartAsync()
        {
            using var client = new System.Net.Sockets.TcpClient("localhost", 5000);
            using NetworkStream ns = client.GetStream();
            using StreamReader reader = new StreamReader(ns);
            using StreamWriter writer = new StreamWriter(ns) { AutoFlush = true };

            string welcomeMessage = await reader.ReadLineAsync();
            Console.WriteLine(welcomeMessage);

            Console.Write("Enter command (Random, Add, Subtract): ");
            string command = Console.ReadLine();
            await writer.WriteLineAsync(command);
            Console.WriteLine($"Sent command: {command}");

            string response = await reader.ReadLineAsync();
            Console.WriteLine($"Received response: {response}");

            Console.Write("Enter two numbers separated by space: ");
            string numbers = Console.ReadLine();
            await writer.WriteLineAsync(numbers);
            Console.WriteLine($"Sent numbers: {numbers}");

            string result = await reader.ReadLineAsync();
            Console.WriteLine($"Received result: {result}");
        }
    }
}