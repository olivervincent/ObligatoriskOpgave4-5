using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Obligatorisk_Opgave_4_5
{
    public class TcpClient
    {
        public async Task StartAsync()
        {
            var testCases = new List<(string Command, string Numbers, string ExpectedOutput)>
            {
                ("Random", "1 10", "Expected result: A number between 1 and 10"),
                ("Add", "3 8", "Expected result: 11"),
                ("Subtract", "19 4", "Expected result: 15"),
                ("Invalid", "5 5", "Expected result: Error message")
            };

            Console.WriteLine("=== TESTING TEXT-BASED PROTOCOL ===");
            
            foreach (var testCase in testCases)
            {
                Console.WriteLine($"\nTesting: {testCase.Command} with numbers {testCase.Numbers}");
                Console.WriteLine(testCase.ExpectedOutput);
                
                try
                {
                    using var client = new System.Net.Sockets.TcpClient("localhost", 7);
                    using NetworkStream ns = client.GetStream();
                    using StreamReader reader = new StreamReader(ns);
                    using StreamWriter writer = new StreamWriter(ns) { AutoFlush = true };

                    string welcomeMessage = await reader.ReadLineAsync();
                    Console.WriteLine($"Server: {welcomeMessage}");
                    
                    await writer.WriteLineAsync(testCase.Command);
                    Console.WriteLine($"Client: {testCase.Command}");

                    string response = await reader.ReadLineAsync();
                    Console.WriteLine($"Server: {response}");
                    
                    await writer.WriteLineAsync(testCase.Numbers);
                    Console.WriteLine($"Client: {testCase.Numbers}");

                    string result = await reader.ReadLineAsync();
                    Console.WriteLine($"Server result: {result}");
                    
                    Console.WriteLine($"Test completed: {testCase.Command}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Test error: {ex.Message}");
                }
                
                await Task.Delay(1000);
            }
            
            Console.WriteLine("\nText-based protocol testing completed.");
        }
    }
}