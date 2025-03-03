using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace Obligatorisk_Opgave_4_5
{
    public class TcpServerJson
    {
        public async Task StartAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8);
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

            string jsonRequest = await reader.ReadLineAsync();
            var request = JsonSerializer.Deserialize<Request>(jsonRequest);
            Console.WriteLine($"Received JSON: {jsonRequest}");

            string result;
            string error = null;

            try
            {
                result = request.method switch
                {
                    "Random" => new Random().Next(request.Tal1, request.Tal2 + 1).ToString(),
                    "Add" => (request.Tal1 + request.Tal2).ToString(),
                    "Subtract" => (request.Tal1 - request.Tal2).ToString(),
                    _ => throw new InvalidOperationException("Invalid command")
                };
            }
            catch (Exception ex)
            {
                result = null;
                error = ex.Message;
            }

            var response = new
            {
                result,
                error
            };

            string jsonResponse = JsonSerializer.Serialize(response);
            await writer.WriteLineAsync(jsonResponse);
            Console.WriteLine($"Sent JSON: {jsonResponse}");

            client.Close();
        }

        private class Request
        {
            public string method { get; set; }
            public int Tal1 { get; set; }
            public int Tal2 { get; set; }
        }
    }
}