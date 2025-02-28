using System;
            using System.Threading.Tasks;
            
            namespace Obligatorisk_Opgave_4_5
            {
                class Program
                {
                    static async Task Main(string[] args)
                    {
                        TcpServer server = new TcpServer();
                        _ = Task.Run(() => server.StartAsync());
            
                        TcpClient client = new TcpClient();
                        await client.StartAsync();
                    }
                }
            }