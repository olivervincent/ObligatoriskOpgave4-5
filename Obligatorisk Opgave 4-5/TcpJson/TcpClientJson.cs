using System;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Obligatorisk_Opgave_4_5
{
    public class TcpClientJson
    {
        public async Task StartAsync()
        {
            var testCases = new List<(string Method, int Tal1, int Tal2, string ExpectedOutput)>
            {
                ("Random", 1, 10, "Expected result: A number between 1 and 10"),
                ("Add", 3, 8, "Expected result: 11"),
                ("Subtract", 19, 4, "Expected result: 15"),
                ("Invalid", 5, 5, "Expected result: Error message")
            };

            Console.WriteLine("=== TESTING JSON PROTOCOL ===");
            
            foreach (var testCase in testCases)
            {
                Console.WriteLine($"\nTesting: {testCase.Method} with numbers {testCase.Tal1} and {testCase.Tal2}");
                Console.WriteLine(testCase.ExpectedOutput);
                
                try
                {
                    using var client = new System.Net.Sockets.TcpClient("localhost", 8);
                    using NetworkStream ns = client.GetStream();
                    using StreamReader reader = new StreamReader(ns);
                    using StreamWriter writer = new StreamWriter(ns) { AutoFlush = true };
                    
                    var request = new
                    {
                        method = testCase.Method,
                        Tal1 = testCase.Tal1,
                        Tal2 = testCase.Tal2
                    };
                    
                    string jsonRequest = JsonSerializer.Serialize(request);
                    await writer.WriteLineAsync(jsonRequest);
                    Console.WriteLine($"Client sent JSON: {jsonRequest}");
                    
                    string jsonResponse = await reader.ReadLineAsync();
                    Console.WriteLine($"Server sent JSON: {jsonResponse}");
                    
                    var response = JsonSerializer.Deserialize<Response>(jsonResponse);
                    
                    if (!string.IsNullOrEmpty(response.error))
                    {
                        Console.WriteLine($"Server returned error: {response.error}");
                    }
                    else
                    {
                        Console.WriteLine($"Server returned result: {response.result}");
                    }
                    
                    Console.WriteLine($"Test completed: {testCase.Method}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Test error: {ex.Message}");
                }
                
                await Task.Delay(1000);
            }
            
            Console.WriteLine("\nJSON protocol testing completed.");
        }

        private class Response
        {
            public string result { get; set; }
            public string error { get; set; }
        }
    }
}