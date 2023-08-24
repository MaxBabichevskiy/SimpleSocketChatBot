using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleSocketChatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Чат-бот запущен...");

            TcpListener server = new TcpListener(IPAddress.Any, 8888);
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(client);
            }
        }

        static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;

            Console.WriteLine("Подключен клиент: " + client.Client.RemoteEndPoint);

            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            string[] responses =
            {
                "Интересно расскажи подробнее.",
                "Как твой день проходит?",
                "Мне кажется, это очень интересная тема.",
                "Действительно?",
                "А что ты думаешь об этом?"
            };

            Random random = new Random();

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string userMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Клиент: " + userMessage);

                int randomIndex = random.Next(responses.Length);
                string botResponse = responses[randomIndex];
                byte[] responseBytes = Encoding.ASCII.GetBytes("Бот: " + botResponse);

                stream.Write(responseBytes, 0, responseBytes.Length);
                Console.WriteLine("Бот: " + botResponse);
            }

            Console.WriteLine("Клиент отключен: " + client.Client.RemoteEndPoint);
            client.Close();
        }
    }
}
